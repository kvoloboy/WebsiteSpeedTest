using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using RequestSpeedTest.BusinessLogic.DTO;
using RequestSpeedTest.BusinessLogic.Exceptions;
using RequestSpeedTest.BusinessLogic.Services;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;

namespace RequestSpeedTest.BusinessLogic.Tests
{
    [TestFixture]
    public class WebsiteSpeedStatisticServiceTests
    {
        private const int Id = 1;

        private ISiteMapService _siteMapService;
        private IUnitOfWork _unitOfWork;
        private IRepository<RequestBenchmarkEntry> _requestRepository;
        private IMapper _mapper;

        private WebsiteSpeedStatisticService _websiteSpeedStatisticService;

        [SetUp]
        public void SetUp()
        {
            _siteMapService = A.Fake<ISiteMapService>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _requestRepository = A.Fake<IRepository<RequestBenchmarkEntry>>();
            _mapper = A.Fake<IMapper>();

            A.CallTo(() => _unitOfWork.GetRepository<RequestBenchmarkEntry>()).Returns(_requestRepository);

            _websiteSpeedStatisticService = new WebsiteSpeedStatisticService(_siteMapService, _unitOfWork, _mapper);
        }

        [Test]
        public void GetAllAsync_ReadsFileFromRepository_Always()
        {
            _websiteSpeedStatisticService.GetAllAsync();

            A.CallTo(() => _requestRepository.FindAllAsync(A<Expression<Func<RequestBenchmarkEntry, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenEntityNotFound()
        {
            A.CallTo(() => _requestRepository.FindSingleAsync(A<Expression<Func<RequestBenchmarkEntry, bool>>>._))
                .Returns((RequestBenchmarkEntry) null);

            Func<Task> action = async () => await _websiteSpeedStatisticService.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<RequestBenchmarkEntryDto>>();
        }

        [Test]
        public async Task GetByIdAsync_ReturnsRequestBenchmarkEntryDto_WhenFound()
        {
            var testDto = GetRequestBenchmarkEntryDto();
            A.CallTo(() => _mapper.Map<RequestBenchmarkEntry, RequestBenchmarkEntryDto>(A<RequestBenchmarkEntry>._))
                .Returns(testDto);

            var entryDto = await _websiteSpeedStatisticService.GetByIdAsync(Id);

            entryDto.Id.Should().Be(Id);
        }

        [Test]
        public void EvaluatePerformanceAsync_CallsSiteMapService_Always()
        {
            var uri = GetUri();

            _websiteSpeedStatisticService.EvaluatePerformanceAsync(uri);

            A.CallTo(() => _siteMapService.GetSiteUrisAsync(uri)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void EvaluatePerformanceAsync_AddsEntryToRepository_Always()
        {
            var uri = GetUri();

            _websiteSpeedStatisticService.EvaluatePerformanceAsync(uri);

            A.CallTo(() => _requestRepository.AddAsync(A<RequestBenchmarkEntry>._))
                .MustHaveHappenedOnceExactly();
        }

        private RequestBenchmarkEntryDto GetRequestBenchmarkEntryDto()
        {
            var dto = new RequestBenchmarkEntryDto
            {
                Id = Id,
                Endpoints = new List<EndpointDto>
                {
                    new EndpointDto()
                }
            };

            return dto;
        }

        private Uri GetUri()
        {
            var uri = new Uri("http://example.com");

            return uri;
        }
    }
}
