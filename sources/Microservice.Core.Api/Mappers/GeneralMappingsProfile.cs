using AutoMapper;
using Microservice.Core.Model;

namespace Microservice.Core.Api.Mappers
{
    public class GeneralMappingsProfile : Profile
    {
        public GeneralMappingsProfile()
        {
            CreateMap(typeof(ResourceRequest<>), typeof(ResourceRequest<>));
        }
    }
}