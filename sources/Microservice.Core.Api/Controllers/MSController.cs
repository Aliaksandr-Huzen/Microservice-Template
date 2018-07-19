using System;
using System.Threading.Tasks;
using AutoMapper;
using Microservice.Core.Extensions;
using Microservice.Core.Model;
using Microservice.Core.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Core.Api.Controllers
{
    [Route(template: "v{version:apiVersion}/[controller]")]
    public class MSController<TModel, TSDO> : Controller
        where TModel : class
        where TSDO : class
    {
        private readonly IService<TSDO> _service;
        protected readonly IMapper Mapper;
        protected readonly IPropertyHelper<TModel> PropertyHelper;

        public MSController(IService<TSDO> service, IMapper mapper, IPropertyHelper<TModel> propertyHelper)
        {
            _service = service;
            Mapper = mapper;
            PropertyHelper = propertyHelper;
        }

        [HttpGet(template: "{resourceKey}")]
        [ProducesResponseType(statusCode: 200)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> Get(Guid resourceKey)
        {
            var response = await _service.Get(resourceKey: resourceKey);

            if (response.Item == null)
            {
                return NotFound();
            }

            var singleResource = Mapper.Map<TModel>(source: response.Item);

            return Ok(value: singleResource);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> Post([FromBody] TModel resourceViewModel)
        {
            var sdo = Mapper.Map<TSDO>(source: resourceViewModel);

            var response = await _service.Create(sdo: sdo);

            var singleResource = Mapper.Map<TModel>(source: response.Item);

            return CreatedAtAction(actionName: nameof(Get), routeValues: new { resourceKey = PropertyHelper.Get<Guid>(obj: singleResource, attributeType: typeof(ResourceKeyAttribute)) }, value: singleResource);
        }

        [HttpPut(template: "{resourceKey}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 400)]
        public async Task<IActionResult> Put(Guid resourceKey, [FromBody] TModel resourceViewModel)
        {
            PropertyHelper.Set(obj: resourceViewModel, attributeType: typeof(ResourceKeyAttribute), propertyValue: resourceKey);

            var sdo = Mapper.Map<TSDO>(source: resourceViewModel);

            await _service.Update(sdo: sdo);

            return NoContent();
        }

        [HttpDelete(template: "{resourceKey}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 404)]
        public async Task<IActionResult> Delete(Guid resourceKey)
        {
            await _service.Delete(resourceKey: resourceKey);

            return NoContent();
        }
    }
}