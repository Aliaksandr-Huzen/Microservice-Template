using System;
using System.Threading.Tasks;

namespace Microservice.Core.Service.DynamoDb
{
    public interface IDynamoDbRepository<TDTO>
        where TDTO : class
    {
        Task<TDTO> Get(Guid resourceKey);

        Task Create(TDTO dto);

        Task Update(TDTO dto);

        Task Delete(Guid resourceKey);
    }
}