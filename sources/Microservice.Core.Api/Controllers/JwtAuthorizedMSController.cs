using AutoMapper;
using Microservice.Core.Extensions;
using Microservice.Core.Service.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Microservice.Core.Api.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JwtAuthorizedMSController<TModel, TSDO> : MSController<TModel, TSDO> 
        where TModel : class 
        where TSDO : class
    {
        public JwtAuthorizedMSController(IService<TSDO> service, IMapper mapper, IPropertyHelper<TModel> propertyHelper) 
            : base(service, mapper, propertyHelper)
        {
        }
    }
}