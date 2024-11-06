using CommonService.RPC;
using CommonService.Services;

namespace UserService.Services
{
    public class UserRpcService(RabbitMQService rabbit, ILogger<UserRpcService> logger) : RpcServiceBase("user-service", rabbit, logger)
    {
        protected override string? Handle(string message, string type, IDictionary<string, object> headers)
        {
            throw new NotImplementedException();
        }

        protected override string? Handle(byte [] message, string type, IDictionary<string, object> headers)
        {
            throw new NotImplementedException();
        }
    }
}
