using System.Net;

namespace CommonService.Services
{
    public class Response<TRes> where TRes : class
    {
        public TRes? Content;
        public HttpStatusCode Code;
    }
}
