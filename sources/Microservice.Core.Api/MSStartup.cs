using AutoMapper;
using Microservice.Core.Api.Binders;
using Microservice.Core.Api.Filters;
using Microservice.Core.Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microservice.Core.Api
{
    public class MSStartup
    {
        protected void MvcServiceSetup(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new ResourceRequestModelBinderProvider());

            options.Filters.Add<ValidationExceptionFilter>();
        }

        protected void SwaggerServiceSetup(SwaggerGenOptions options)
        {
            options.SchemaFilter<ResourceRequestSchemaFilter>();
        }

        protected void AutoMapperServiceSetup(IMapperConfigurationExpression options)
        {
            options.AddProfile<GeneralMappingsProfile>();
        }
    }
}