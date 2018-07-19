using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Microservice.Core.Service.Exceptions;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbRepository<TDTO> : IDynamoDbRepository<TDTO> 
        where TDTO : class
    {
        protected readonly IDynamoDBContext DynamoDbContext;
        private readonly IDynamoDbResourcePolicy<TDTO> _dynamoDbResourcePolicy;

        public DynamoDbRepository(IDynamoDBContext dynamoDbContext,
            IDynamoDbResourcePolicy<TDTO> dynamoDbResourcePolicy)
        {
            DynamoDbContext = dynamoDbContext;
            _dynamoDbResourcePolicy = dynamoDbResourcePolicy;
        }

        public Task<TDTO> Get(Guid resourceKey)
        {
            return DynamoDbContext.LoadAsync<TDTO>(resourceKey);
        }

        public async Task Create(TDTO dto)
        {
            var table = DynamoDbContext.GetTargetTable<TDTO>();

            var document = DynamoDbContext.ToDocument(dto);

            var expectedState = _dynamoDbResourcePolicy.GetExpectedState(false, dto);

            try
            {
                await table.PutItemAsync(document, new PutItemOperationConfig {ExpectedState = expectedState});
            }
            catch (ConditionalCheckFailedException)
            {
                throw new ValidationException(ErrorCodes.DuplicateItem, $"The same key {_dynamoDbResourcePolicy.GetKey(dto)} already exists");
            }
        }

        public async Task Update(TDTO dto)
        {
            var table = DynamoDbContext.GetTargetTable<TDTO>();

            var document = DynamoDbContext.ToDocument(dto);

            var expectedState = _dynamoDbResourcePolicy.GetExpectedState(true, dto);


            try
            {
                await table.PutItemAsync(document, new PutItemOperationConfig { ExpectedState = expectedState });
            }
            catch (ConditionalCheckFailedException)
            {
                throw new ValidationException(ErrorCodes.ItemNotFound, $"The same key {_dynamoDbResourcePolicy.GetKey(dto)} already exists");
            }
        }

        public Task Delete(Guid resourceKey)
        {
           return DynamoDbContext.DeleteAsync(resourceKey);
        }
    }
}