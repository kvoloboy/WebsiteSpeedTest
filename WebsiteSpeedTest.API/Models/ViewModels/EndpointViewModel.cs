﻿using System;

 namespace RequestSpeedTest.API.Models.ViewModels
{
    public class EndpointViewModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Uri { get; set; }
        public int ResponseTime { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
    }
}
