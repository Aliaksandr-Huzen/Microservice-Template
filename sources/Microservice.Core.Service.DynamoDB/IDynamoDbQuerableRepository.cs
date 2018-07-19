using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Core.Model;

namespace Microservice.Core.Service.DynamoDb
{
    public interface IDynamoDbQuerableRepository<TServiceQuery, TDTO> : IDynamoDbRepository<TDTO>
        where TDTO : class 
        where TServiceQuery : class
    {
        Task<ICollection<TDTO>> Get(ResourceRequest<TServiceQuery> request);
    }
}