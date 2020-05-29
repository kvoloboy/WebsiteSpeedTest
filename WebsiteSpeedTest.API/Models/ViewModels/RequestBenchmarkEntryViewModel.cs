using System.Collections.Generic;

namespace RequestSpeedTest.API.Models.ViewModels
{
    public class RequestBenchmarkEntryViewModel
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public int MinResponseTime { get; set; }
        public int MaxResponseTime { get; set; }
        public IEnumerable<EndpointViewModel> Endpoints { get; set; }
    }
}
