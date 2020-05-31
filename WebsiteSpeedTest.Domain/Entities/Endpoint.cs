namespace RequestSpeedTest.Domain.Entities
{
    public class Endpoint
    {
        public string Uri { get; set; }
        public int ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
