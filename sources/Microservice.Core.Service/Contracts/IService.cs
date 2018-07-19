using System;
using System.Threading.Tasks;
using Microservice.Core.Model;

namespace Microservice.Core.Service.Contracts
{
    public interface IService<TSDO>
        where TSDO : class
    {
        Task<ResourceSingleResponse<TSDO>> Get(Guid resourceKey);

        Task<ResourceSingleResponse<TSDO>> Create(TSDO sdo);

        Task<ResourceSingleResponse<TSDO>> Update(TSDO sdo);

        Task Delete(Guid resourceKey);
    }
}