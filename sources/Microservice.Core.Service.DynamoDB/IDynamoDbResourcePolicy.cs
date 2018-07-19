using Amazon.DynamoDBv2.DocumentModel;

namespace Microservice.Core.Service.DynamoDb
{
    public interface IDynamoDbResourcePolicy<in TDTO>
        where TDTO : class
    {
        string GetKey(TDTO dto);

        ExpectedState GetExpectedState(bool exist, TDTO dto);
    }
}