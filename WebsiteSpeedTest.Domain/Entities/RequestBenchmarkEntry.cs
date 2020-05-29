using System.Collections.Generic;

namespace RequestSpeedTest.Domain.Entities
{
    public class RequestBenchmarkEntry : BaseEntity
    {
        public string Uri { get; set; }
        public List<Endpoint> Endpoints { get; set; }
    }
}
