using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using RequestSpeedTest.BusinessLogic.DTO;
using RequestSpeedTest.BusinessLogic.Exceptions;
using RequestSpeedTest.BusinessLogic.Extensions;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;

namespace RequestSpeedTest.BusinessLogic.Services
{
    public class WebsiteSpeedStatisticService : IWebsiteSpeedStatisticService
    {
        private readonly ISiteMapService _siteMapService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<RequestBenchmarkEntry> _requestBenchmarkRepository;

        public WebsiteSpeedStatisticService(ISiteMapService siteMapService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _siteMapService = siteMapService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _requestBenchmarkRepository = _unitOfWork.GetRepository<RequestBenchmarkEntry>();
        }

        public async Task<RequestBenchmarkEntryDto> EvaluatePerformanceAsync(Uri siteUri)
        {
            var siteUris = (await _siteMapService.GetSiteUrisAsync(siteUri)).ToList();
            var benchmarkEntry = await GetBenchmarkDetailsAsync(siteUris, siteUri);

            await _requestBenchmarkRepository.AddAsync(benchmarkEntry);
            await _unitOfWork.CommitAsync();

            var requestDto = _mapper.Map<RequestBenchmarkEntry, RequestBenchmarkEntryDto>(benchmarkEntry);

            PrepareRequestDto()(requestDto);

            return requestDto;
        }

        public async Task<RequestBenchmarkEntryDto> GetByIdAsync(int id)
        {
            var requestBenchmarkEntry = await _requestBenchmarkRepository.FindSingleAsync(r => r.Id == id);

            if (requestBenchmarkEntry == null)
            {
                throw new EntityNotFoundException<RequestBenchmarkEntryDto>(id);
            }

            var requestDto = _mapper.Map<RequestBenchmarkEntry, RequestBenchmarkEntryDto>(requestBenchmarkEntry);

            PrepareRequestDto()(requestDto);

            return requestDto;
        }

        public async Task<IEnumerable<RequestBenchmarkEntryDto>> GetAllAsync()
        {
            var requests = await _requestBenchmarkRepository.FindAllAsync();
            var requestsDto = _mapper.Map<List<RequestBenchmarkEntry>, List<RequestBenchmarkEntryDto>>(requests);

            requestsDto.ForEach(PrepareRequestDto());

            return requestsDto;
        }

        private static async Task<RequestBenchmarkEntry> GetBenchmarkDetailsAsync(
            IEnumerable<Uri> siteUris,
            Uri requestUri)
        {
            var client = new HttpClient();
            var endpoints = new BlockingCollection<Endpoint>();

            var request = new RequestBenchmarkEntry
            {
                Uri = requestUri.ToString()
            };

            await siteUris.ParallelForEachAsync(
                EvaluateResponseTimeAsync(client, endpoints),
                Environment.ProcessorCount);

            request.Endpoints = endpoints.ToList();

            return request;
        }

        private static Func<Uri, Task> EvaluateResponseTimeAsync(
            HttpClient client,
            BlockingCollection<Endpoint> endpoints)
        {
            async Task Func(Uri uri)
            {
                var sw = new Stopwatch();
                var success = true;
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

                sw.Start();
                var responseTask = client.GetAsync(uri);

                try
                {
                    response = await responseTask;
                }
                catch (HttpRequestException)
                {
                    success = false;
                }

                sw.Stop();
                AddBenchmarkDetails(uri, sw.Elapsed.Milliseconds, (int) response.StatusCode, success, endpoints);
            }

            return Func;
        }

        private static Action<RequestBenchmarkEntryDto> PrepareRequestDto()
        {
            void Action(RequestBenchmarkEntryDto dto)
            {
                if (!dto.Endpoints.Any())
                {
                    return;
                }

                dto.MinResponseTime = dto.Endpoints.Min(endpoint => endpoint.ResponseTime);
                dto.MaxResponseTime = dto.Endpoints.Max(endpoint => endpoint.ResponseTime);
                dto.Endpoints = dto.Endpoints.OrderBy(endpoint => endpoint.ResponseTime);
            }

            return Action;
        }

        private static void AddBenchmarkDetails(
            Uri uri,
            int responseTime,
            int statusCode,
            bool success,
            BlockingCollection<Endpoint> endpoints)
        {
            var endpoint = new Endpoint
            {
                Uri = uri.AbsoluteUri,
                ResponseTime = responseTime,
                StatusCode = statusCode,
                Success = success
            };

            endpoints.Add(endpoint);
        }
    }
}
