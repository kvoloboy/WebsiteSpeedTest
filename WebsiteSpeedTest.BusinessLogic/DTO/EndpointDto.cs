using System;

namespace RequestSpeedTest.BusinessLogic.DTO
{
    public class EndpointDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Uri { get; set; }
        public int ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
