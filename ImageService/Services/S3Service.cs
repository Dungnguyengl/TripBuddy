using Amazon.S3;
using Amazon.S3.Model;
using CommonService.Extentions;
using ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Services
{
    public class S3Service
    {
        private readonly AmazonS3Client _s3Client;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;

        public S3Service(IConfiguration configuration, ILogger<S3Service> logger, IServiceProvider services)
        {
            _configuration = configuration;

            var options = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_configuration ["AWS:Region"])
            };
            _s3Client = new AmazonS3Client(_configuration ["AWS:AccessKey"], _configuration ["AWS:SecretKey"], options);

            _logger = logger;
            _services = services;
        }

        public async Task<Guid> StoreImageAsync(Stream imageStream, Guid key)
        {
            using var inputStream = new MemoryStream();
            imageStream.Position = 0;
            imageStream.CopyTo(inputStream);
            var bucketName = _configuration ["S3:BucketName"];
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = $"imgs/{key}.jpeg",
                InputStream = inputStream,
                ContentType = "image/jpeg",
                CannedACL = S3CannedACL.NoACL,
                AutoCloseStream = true
            };
            await RetryExtentions.Retry(async () =>
            {
                var response = await _s3Client.PutObjectAsync(putRequest);
                _logger.LogInformation(response.ToString());
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception("Failed to upload image to S3.");
            });

            await SaveToDatabaseAsync(key);

            return key;
        }

        public string GeneratePreSignedURL(Guid key)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            if (!context.ImageHeads.Any(x => x.ImageKey == key && (!x.IsDelete ?? false)))
            {
                throw new Exception($"Not found image {key}!");
            }

            var bucketName = _configuration ["S3:BucketName"];
            var expiryInMinutes = _configuration ["S3:PreSignedUrlExpiry"].ToInt();
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = $"imgs/{key}.jpeg",
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public async Task DeleteImage(Guid key)
        {

            await MarkAsDeletedInDatabaseAsync(key);
        }

        public async Task RestoreImage(Guid key)
        {
              await RestoreInDatabase(key);
        }

        public async Task DeleteForeverAsync()
        {
            var bucketName = _configuration ["S3:BucketName"];
            var daysToKeepDeleted = _configuration ["S3:DaysToKeepDeleted"].ToInt();

            var deletedImages = GetDeletedImages(daysToKeepDeleted);

            foreach (var (Key, DeleteTime) in deletedImages)
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = Key.ToString()
                };

                var response = await _s3Client.DeleteObjectAsync(deleteRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    await DeleteRecordFromDatabase(Key);
                }
            }
        }

        private async Task SaveToDatabaseAsync(Guid key)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            var image = new ImageHead
            {
                ImageKey = key,
                ImageType = "jpeg"
            };
            context.ImageHeads.Add(image);
            await context.SaveChangesAsync();
        }

        private async Task MarkAsDeletedInDatabaseAsync(Guid key)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            var image = await context.ImageHeads
                .AsNoTracking()
                .FirstOrDefaultAsync(img => img.ImageKey == key);
            
            ArgumentNullException.ThrowIfNull(image, nameof(MarkAsDeletedInDatabaseAsync));
            
            image.IsDelete = true;
            image.DeleteDate = DateTime.Now;

            context.ImageHeads.Update(image);
            await context.SaveChangesAsync();
        }

        private async Task RestoreInDatabase(Guid key)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            var image = await context.ImageHeads
                .AsNoTracking()
                .FirstOrDefaultAsync(img => img.ImageKey == key);

            ArgumentNullException.ThrowIfNull(image, nameof(MarkAsDeletedInDatabaseAsync));

            image.IsDelete = false;
            image.DeleteDate = null;

            context.ImageHeads.Update(image);
            await context.SaveChangesAsync();
        }

        private List<(Guid Key, DateTime? DeleteTime)> GetDeletedImages(int daysToKeepDeleted)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            return context.ImageHeads
                .AsNoTracking()
                .Where(image => (image.IsDelete ?? false)
                                && image.DeleteDate.HasValue
                                && image.DeleteDate.Value.AddDays(daysToKeepDeleted) >= DateTime.Now)
                .ToList()
                .Select(image => (Key: image.ImageKey, DeleteTime: image.DeleteDate))
                .ToList();
        }

        private async Task DeleteRecordFromDatabase(Guid key)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageServiceDbContext>();
            var image = await context.ImageHeads
                .AsNoTracking()
                .FirstOrDefaultAsync(img => img.ImageKey == key);

            ArgumentNullException.ThrowIfNull(image, nameof(MarkAsDeletedInDatabaseAsync));

            context.ImageHeads.Remove(image);
            await context.SaveChangesAsync();
        }
    }
}
