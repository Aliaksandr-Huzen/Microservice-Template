using Microsoft.Extensions.Configuration;
using StructureMap;

namespace Microservice.Core.Api.Version1.IoC
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry(IConfiguration configuration)
        {
        }
    }
}