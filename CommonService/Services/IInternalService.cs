using CommonService.Constants;

namespace CommonService.Services
{
    public interface IInternalService
    {
        public Task<Response<TResult>> GetAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class;
        public Task<ODataRespose<TResult>> GetByODataAsync<TResult>(ServiceType type, ODataParam? param = null, string path = "") where TResult : class;
        public Task<Response<TResult>> PostAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class;
        public Task<Response<TResult>> PushAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class;
        public Task<Response<TResult>> DeleteAsync<TResult>(ServiceType type, object? param, string path = "") where TResult : class;
    }
}
