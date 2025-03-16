using Shares.ServiceContracts;

namespace GrpcService.Services.Implements
{
    public abstract class BaseService
    {
        protected ResponseObj<T> CreateResponse<T>(T? data, string message)
        {
            return new ResponseObj<T>
            {
                Data = data,
                Message = message
            };
        }
    }
}
