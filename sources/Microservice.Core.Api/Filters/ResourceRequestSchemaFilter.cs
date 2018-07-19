using System.Reflection;
using Microservice.Core.Model;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microservice.Core.Api.Filters
{
    public class ResourceRequestSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (context.SystemType.GetTypeInfo().IsGenericType && context.SystemType.GetTypeInfo().GetGenericTypeDefinition() == typeof(ResourceRequest<>))
            {

            }
        }
    }
}