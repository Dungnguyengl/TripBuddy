﻿

using CommonService.Constants;
using CommonService.Extentions;
using Newtonsoft.Json;
using Steeltoe.Discovery;
using System.Net.Http.Json;
using System.Text;

namespace CommonService.Services
{
    public class InternalService(HttpClient client, IDiscoveryClient discovery) : ServiceBase(client, discovery), IInternalService
    {
        public async Task<Response<TResult>> DeleteAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class
        {
            var jsonString = JsonConvert.SerializeObject(param);
            var req = new HttpRequestMessage(HttpMethod.Delete, GetEndPoint(type, path))
            {
                Content = JsonContent.Create(param),
            };
            var result = await Client.SendAsync(req);

            return new Response<TResult>
            {
                Content = await result.Content.ReadFromJsonAsync<TResult>(),
                Code = result.StatusCode
            };
        }

        public async Task<Response<TResult>> GetAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class
        {
            StringBuilder paramsBuilder = new(path);
            if (param is not null)
            {
                paramsBuilder.Append('?');
                var paramType = param.GetType();
                var properties = paramType.GetProperties().Select(property =>
                {
                    var name = property.Name?.ToCamelCase();
                    var value = property.GetValue(param);
                    return $"{name}={value}";
                });
                paramsBuilder.Append(string.Join('&', properties));
            }

            var result = await Client.GetAsync(GetEndPoint(type, paramsBuilder.ToString()));

            var resContentRaw = await result.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<TResult>(resContentRaw);

            return new Response<TResult>
            {
                Content = content,
                Code = result.StatusCode
            };
        }

        public async Task<Response<TResult>> PostAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class
        {
            var jsonString = JsonConvert.SerializeObject(param);
            var req = new HttpRequestMessage(HttpMethod.Post, GetEndPoint(type, path))
            {
                Content = JsonContent.Create(param),
            };
            var result = await Client.SendAsync(req);

            return new Response<TResult>
            {
                Content = await result.Content.ReadFromJsonAsync<TResult>(),
                Code = result.StatusCode
            };
        }

        public async Task<Response<TResult>> PushAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class
        {
            var jsonString = JsonConvert.SerializeObject(param);
            var req = new HttpRequestMessage(HttpMethod.Put, GetEndPoint(type, path))
            {
                Content = JsonContent.Create(param),
            };
            var result = await Client.SendAsync(req);

            return new Response<TResult>
            {
                Content = await result.Content.ReadFromJsonAsync<TResult>(),
                Code = result.StatusCode
            };
        }
    }
}
