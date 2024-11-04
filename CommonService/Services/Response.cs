using System.Net;

namespace CommonService.Services
{
    public class Response<TRes> where TRes : class
    {
        public TRes Content;
        public HttpStatusCode Code;
    }

    public class ODataRespose<TRes> where TRes : class
    {
        public ICollection<TRes> Value {get; set;}
        public int Count {get; set;}
        public HttpStatusCode Code;
    }
}
