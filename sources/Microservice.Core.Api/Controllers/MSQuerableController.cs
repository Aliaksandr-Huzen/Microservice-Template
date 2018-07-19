using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Core.Extensions;
using Microservice.Core.Model;
using Microservice.Core.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Core.Api.Controllers
{
    public class MSQuerableController<TQuery, TServiceQuery, TModel, TSDO> : MSController<TModel, TSDO>
        where TQuery : class
        where TServiceQuery : class 
        where TModel : class
        where TSDO : class
    {
        private readonly IQuerableService<TServiceQuery, TSDO> _service;

        public MSQuerableController(IQuerableService<TServiceQuery, TSDO> service, IMapper mapper, IPropertyHelper<TModel> propertyHelper) 
            : base(service: service, mapper: mapper, propertyHelper: propertyHelper)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> Get(ResourceRequest<TQuery> resourceRequest)
        {
            var serviceResourceRequest = Mapper.Map<ResourceRequest<TServiceQuery>>(source: resourceRequest);

            var response = await _service.Get(request: serviceResourceRequest);

            var multipleResource = Mapper.Map<ICollection<TModel>>(source: response.Items);

            return Ok(value: multipleResource);
        }
    }
}