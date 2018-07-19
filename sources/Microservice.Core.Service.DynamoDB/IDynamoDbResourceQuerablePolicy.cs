namespace Microservice.Core.Service.DynamoDb
{
    public interface IDynamoDbResourceQuerablePolicy<in TServiceQuery, in TDTO> : IDynamoDbResourcePolicy<TDTO>
        where TDTO : class
        where TServiceQuery : class
    {
        DynamoDbScanParameters Convert(TServiceQuery serviceQuery);
    }
}