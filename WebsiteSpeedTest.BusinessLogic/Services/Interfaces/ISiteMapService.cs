using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RequestSpeedTest.BusinessLogic.Services.Interfaces
{
    public interface ISiteMapService
    {
        Task<IEnumerable<Uri>> GetSiteUrisAsync(Uri siteUri);
    }
}
