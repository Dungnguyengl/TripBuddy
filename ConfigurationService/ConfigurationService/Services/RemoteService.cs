using Renci.SshNet;

namespace ConfigurationService.Services
{
    public class RemoteService : IDisposable
    {
        private readonly SshClient _client;

        public RemoteService(string host, string username, string keyFilePaths)
        {
            if (!File.Exists(keyFilePaths))
            {
                throw new ArgumentException("Not contant key");
            }

            var privateKey = new PrivateKeyFile(File.OpenRead(keyFilePaths));
            var keyFiles = new[] { privateKey };
            _client = new SshClient(host, username, keyFiles);
            _client.Connect();
        }

        public void Dispose()
        {
            _client.Disconnect();
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task RunCommand (string command)
        {
            var cmd = _client.CreateCommand(command);
            await cmd.ExecuteAsync();
        }
    }
}
