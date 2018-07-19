using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Microservice.Core.Api.Version1.IoC
{
    public class ApiContainer : Container
    {
        public ApiContainer(IConfigurationRoot configuration, string environment)
        {
            Container = new Container(config =>
            {
                config.For<IConfigurationRoot>().Singleton().Use(configuration);
                config.AddRegistry(new AwsRegistry(configuration, environment));
                config.AddRegistry(new ApiRegistry(configuration));
                config.AddRegistry(new ServicesRegistry(configuration));
            });
        }

        public IContainer Container { get; }
    }
}