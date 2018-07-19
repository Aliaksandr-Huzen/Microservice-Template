using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Microservice.Core.Api.Version1.IoC
{
    public class AwsRegistry : Registry
    {
        public AwsRegistry(IConfiguration configuration, string environment)
        {
            var isDevelopment = string.Equals(EnvironmentName.Development, environment, StringComparison.InvariantCulture);

            if (isDevelopment)
            {
                var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:8000" };
                For<IAmazonDynamoDB>().AlwaysUnique().Use(context => new AmazonDynamoDBClient("fake", "fake", clientConfig));
            }
            else
            {
                throw new NotSupportedException("Aws is not protuction-eeady yet");
            }

            For<IDynamoDBContext>().Singleton().Use(context => new DynamoDBContext(
                context.GetInstance<IAmazonDynamoDB>(),
                new DynamoDBContextConfig() { TableNamePrefix = configuration[AppConfigConsts.AwsTablePrefix] }));
        }
    }
}