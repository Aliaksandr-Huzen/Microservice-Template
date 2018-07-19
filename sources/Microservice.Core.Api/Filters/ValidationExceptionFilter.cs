using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microservice.Core.Service.Exceptions;

namespace Microservice.Core.Api.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.ExceptionHandled = true;

                context.Result = new BadRequestObjectResult(
                new
                {
                    validationException.Code,
                    validationException.Message
                });
            }
        }
    }
}