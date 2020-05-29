using AutoMapper;
using RequestSpeedTest.API.Models.ViewModels;
using RequestSpeedTest.BusinessLogic.DTO;

namespace RequestSpeedTest.API.Mappings
{
    public class DtoToViewModelProfile : Profile
    {
        public DtoToViewModelProfile()
        {
            CreateMap<EndpointDto, EndpointViewModel>();

            CreateMap<RequestBenchmarkEntryDto, RequestBenchmarkEntryViewModel>();
        }
    }
}
