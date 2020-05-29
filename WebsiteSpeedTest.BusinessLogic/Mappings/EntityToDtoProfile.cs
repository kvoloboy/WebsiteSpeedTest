using AutoMapper;
using RequestSpeedTest.BusinessLogic.DTO;
using RequestSpeedTest.Domain.Entities;

namespace RequestSpeedTest.BusinessLogic.Mappings
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<Endpoint, EndpointDto>();

            CreateMap<RequestBenchmarkEntry, RequestBenchmarkEntryDto>();
        }
    }
}
