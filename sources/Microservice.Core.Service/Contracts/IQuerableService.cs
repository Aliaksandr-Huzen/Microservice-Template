using System.Threading.Tasks;
using Microservice.Core.Model;

namespace Microservice.Core.Service.Contracts
{
    public interface IQuerableService<TServiceQuery, TSDO> : IService<TSDO>
        where TServiceQuery : class
        where TSDO : class
    {
        Task<ResourceMultipleResponse<TSDO>> Get(ResourceRequest<TServiceQuery> request);
    }
}