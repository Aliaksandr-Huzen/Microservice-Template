using AutoMapper;
using Microservice.Core.Extensions;
using Microservice.Core.Service.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Microservice.Core.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JwtAuthorizedMSQuerableController<TQuery, TServiceQuery, TModel, TSDO> : MSQuerableController<TQuery, TServiceQuery, TModel, TSDO> 
        where TQuery : class 
        where TServiceQuery : class 
        where TModel : class 
        where TSDO : class
    {
        public JwtAuthorizedMSQuerableController(IQuerableService<TServiceQuery, TSDO> service, IMapper mapper, IPropertyHelper<TModel> propertyHelper) 
            : base(service: service, mapper: mapper, propertyHelper: propertyHelper)
        {
        }
    }
}