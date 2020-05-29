using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RequestSpeedTest.BusinessLogic.DTO;

namespace RequestSpeedTest.BusinessLogic.Services.Interfaces
{
    public interface IWebsiteSpeedStatisticService
    {
        Task<RequestBenchmarkEntryDto> EvaluatePerformanceAsync(Uri siteUri);
        Task<RequestBenchmarkEntryDto> GetByIdAsync(int id);
        Task<IEnumerable<RequestBenchmarkEntryDto>> GetAllAsync();
    }
}
