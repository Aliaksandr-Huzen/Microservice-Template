using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microservice.Core.Model;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbQuerableRepository<TServiceQuery, TDTO> : DynamoDbRepository<TDTO>, IDynamoDbQuerableRepository<TServiceQuery, TDTO>
        where TDTO : class 
        where TServiceQuery : class
    {
        private readonly IDynamoDbResourceQuerablePolicy<TServiceQuery, TDTO> _dynamoDbResourceQuerablePolicy;

        public DynamoDbQuerableRepository(IDynamoDBContext dynamoDbContext, IDynamoDbResourceQuerablePolicy<TServiceQuery, TDTO> dynamoDbResourceQuerablePolicy) 
            : base(dynamoDbContext, dynamoDbResourceQuerablePolicy)
        {
            _dynamoDbResourceQuerablePolicy = dynamoDbResourceQuerablePolicy;
        }

        public async Task<ICollection<TDTO>> Get(ResourceRequest<TServiceQuery> request)
        {
            var policy = _dynamoDbResourceQuerablePolicy.Convert(request?.Query);

            var result =  await DynamoDbContext.ScanAsync<TDTO>(
                 policy.ScanConditions,
                 policy.OperationConfig
                ).GetRemainingAsync();

            return result;
        }
    }
}