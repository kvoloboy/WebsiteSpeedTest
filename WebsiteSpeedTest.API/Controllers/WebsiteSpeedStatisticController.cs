using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RequestSpeedTest.API.Models.ViewModels;
using RequestSpeedTest.BusinessLogic.DTO;
using RequestSpeedTest.BusinessLogic.Exceptions;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;

namespace RequestSpeedTest.API.Controllers
{
    [ApiController]
    [Route("api/website-statistic")]
    public class WebsiteSpeedStatisticController : ControllerBase
    {
        private readonly IWebsiteSpeedStatisticService _websiteSpeedStatisticService;
        private readonly IMapper _mapper;
        private readonly ILogger<WebsiteSpeedStatisticController> _logger;

        public WebsiteSpeedStatisticController(
            IWebsiteSpeedStatisticService websiteSpeedStatisticService,
            IMapper mapper,
            ILogger<WebsiteSpeedStatisticController> logger)
        {
            _websiteSpeedStatisticService = websiteSpeedStatisticService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> EvaluatePerformanceAsync(
            [FromBody] EvaluatePerformanceRequestModel requestModel)
        {
            var uri = new Uri(requestModel.Uri);
            var requestDetails = await _websiteSpeedStatisticService.EvaluatePerformanceAsync(uri);
            var viewModel = _mapper.Map<RequestBenchmarkEntryDto, RequestBenchmarkEntryViewModel>(requestDetails);

            _logger.LogDebug($"Evaluated website performance by uri: {requestModel.Uri}");

            return Ok(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var requestsDto = await _websiteSpeedStatisticService.GetAllAsync();
            var viewModels =
                _mapper.Map<IEnumerable<RequestBenchmarkEntryDto>, IEnumerable<RequestBenchmarkEntryViewModel>>(
                    requestsDto);

            _logger.LogDebug("Reading requests history");

            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            RequestBenchmarkEntryDto requestDto;

            try
            {
                requestDto = await _websiteSpeedStatisticService.GetByIdAsync(id);
            }
            catch (EntityNotFoundException<RequestBenchmarkEntryDto>)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<RequestBenchmarkEntryDto, RequestBenchmarkEntryViewModel>(requestDto);

            _logger.LogDebug($"Reading website performance stats by id: {id}");

            return Ok(viewModel);
        }
    }
}
