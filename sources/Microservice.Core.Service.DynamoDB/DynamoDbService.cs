using System;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Core.Model;
using Microservice.Core.Service.Contracts;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbService<TSDO, TDTO> : IService<TSDO>
        where TSDO : class 
        where TDTO : class
    {
        private readonly IDynamoDbRepository<TDTO> _dynamoDbRepository;
        protected readonly IMapper Mapper;

        public DynamoDbService(IDynamoDbRepository<TDTO> dynamoDbRepository, IMapper mapper)
        {
            _dynamoDbRepository = dynamoDbRepository;
            Mapper = mapper;
        }

        public Task<ResourceSingleResponse<TSDO>> Get(Guid resourceKey)
        {
            return DoAction(() => _dynamoDbRepository.Get(resourceKey));
        }

        public Task<ResourceSingleResponse<TSDO>> Create(TSDO sdo)
        {
            var dto = Mapper.Map<TDTO>(sdo);

            return DoAction(async () =>
            {
                await _dynamoDbRepository.Create(dto);
                return dto;
            });
        }

        public Task<ResourceSingleResponse<TSDO>> Update(TSDO sdo)
        {
            var dto = Mapper.Map<TDTO>(sdo);

            return DoAction(async () =>
            {
                await _dynamoDbRepository.Update(dto);
                return dto;
            });
        }

        public Task Delete(Guid resourceKey)
        {
            return _dynamoDbRepository.Delete(resourceKey);
        }

        protected  async Task<ResourceSingleResponse<TSDO>> DoAction(Func<Task<TDTO>> func)
        {
            var dto = await func();

            return new ResourceSingleResponse<TSDO>
            {
                Item = Mapper.Map<TSDO>(dto)
            };
        }
    }
}