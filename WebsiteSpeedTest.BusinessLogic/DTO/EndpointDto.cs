using System;

namespace RequestSpeedTest.BusinessLogic.DTO
{
    public class EndpointDto
    {
        public string Uri { get; set; }
        public int ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
