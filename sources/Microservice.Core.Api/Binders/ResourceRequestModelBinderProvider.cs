using System;
using Microservice.Core.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Microservice.Core.Api.Binders
{
    public class ResourceRequestModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(paramName: nameof(context));
            }

            var modelType = context.Metadata.ModelType;

            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(ResourceRequest<>))
            {
                return new BinderTypeModelBinder(binderType: typeof(ResourceRequestModelBinder));
            }

            return null;
        }
    }
}