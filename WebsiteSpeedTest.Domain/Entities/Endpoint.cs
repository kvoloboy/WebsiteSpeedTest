using System;

namespace RequestSpeedTest.Domain.Entities
{
    public class Endpoint : BaseEntity
    {
        public string Uri { get; set; }
        public int ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }

        public int RequestBenchmarkEntryId { get; set; }
        public RequestBenchmarkEntry RequestBenchmarkEntry { get; set; }
    }
}
