using System.Collections.Generic;

namespace RequestSpeedTest.BusinessLogic.DTO
{
    public class RequestBenchmarkEntryDto
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public int MinResponseTime { get; set; }
        public int MaxResponseTime { get; set; }
        public IEnumerable<EndpointDto> Endpoints { get; set; }
    }
}
