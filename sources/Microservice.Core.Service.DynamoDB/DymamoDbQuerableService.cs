using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Core.Model;
using Microservice.Core.Service.Contracts;

namespace Microservice.Core.Service.DynamoDb
{
    public class DymamoDbQuerableService<TServiceQuery, TSDO, TDTO> : DynamoDbService<TSDO, TDTO>, IQuerableService<TServiceQuery, TSDO>
        where TServiceQuery : class
        where TSDO : class
        where TDTO : class 
    {
        private readonly IDynamoDbQuerableRepository<TServiceQuery, TDTO> _dynamoDbQuerableRepository;

        public DymamoDbQuerableService(IDynamoDbQuerableRepository<TServiceQuery, TDTO> dynamoDbQuerableRepository, IMapper mapper) : base(dynamoDbQuerableRepository, mapper)
        {
            _dynamoDbQuerableRepository = dynamoDbQuerableRepository;
        }

        public async Task<ResourceMultipleResponse<TSDO>> Get(ResourceRequest<TServiceQuery> request)
        {
            var dto = await _dynamoDbQuerableRepository.Get(request);

            return new ResourceMultipleResponse<TSDO>()
            {
                Items = Mapper.Map<ICollection<TSDO>>(dto)
            };
        }
    }
}