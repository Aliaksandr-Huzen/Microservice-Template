using Microservice.Core.Extensions;
using Microservice.Core.Service.DynamoDb;
using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Microservice.Core.Api.Version1.IoC
{
    public class ApiRegistry : Registry
    {
        public ApiRegistry(IConfiguration configuration)
        {
            For(typeof(IDynamoDbRepository<>)).Singleton().Use(typeof(DynamoDbRepository<>));
            For(typeof(IDynamoDbResourcePolicy<>)).Singleton().Use(typeof(DynamoDbResourcePolicy<>));

            For(typeof(IDynamoDbQuerableRepository<,>)).Singleton().Use(typeof(DynamoDbQuerableRepository<,>));
            For(typeof(IDynamoDbResourceQuerablePolicy<,>)).Singleton().Use(typeof(DynamoDbResourceQuerablePolicy<,>));

            For(typeof(IPropertyHelper<>)).Singleton().Use(typeof(PropertyHelper<>));
        }
    }
}