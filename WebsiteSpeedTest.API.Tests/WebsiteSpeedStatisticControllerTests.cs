using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RequestSpeedTest.API.Controllers;
using RequestSpeedTest.API.Models.ViewModels;
using RequestSpeedTest.BusinessLogic.DTO;
using RequestSpeedTest.BusinessLogic.Exceptions;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;

namespace RequestSpeedTest.API.Tests
{
    [TestFixture]
    public class WebsiteSpeedStatisticControllerTests
    {
        private const int Id = 1;
        private const string Host = "example.com";

        private IWebsiteSpeedStatisticService _requestStatisticService;
        private ILogger<WebsiteSpeedStatisticController> _logger;
        private IMapper _mapper;

        private WebsiteSpeedStatisticController _websiteStatisticController;

        [SetUp]
        public void Setup()
        {
            _requestStatisticService = A.Fake<IWebsiteSpeedStatisticService>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WebsiteSpeedStatisticController>>();

            _websiteStatisticController = new WebsiteSpeedStatisticController(_requestStatisticService, _mapper, _logger);
        }

        [Test]
        public async Task EvaluatePerformanceAsync_CallsEvaluationService_WhenValidParameters()
        {
            var requestViewModel = GetEvaluatePerformanceRequestModel();
            Expression<Func<Uri, bool>> matchPredicate = uri =>
                uri.Host == Host;

            await _websiteStatisticController.EvaluatePerformanceAsync(requestViewModel);

            A.CallTo(() => _requestStatisticService.EvaluatePerformanceAsync(A<Uri>.That.Matches(matchPredicate)))
                .MustHaveHappenedOnceExactly();
        }


        [Test]
        public async Task EvaluatePerformanceAsync_ReturnsWebsiteViewModel_WhenEvaluationIsSucceed()
        {
            var requestViewModel = GetEvaluatePerformanceRequestModel();

            var result = await _websiteStatisticController.EvaluatePerformanceAsync(requestViewModel);

            result.Should().BeOkObjectResult();
        }

        [Test]
        public async Task GetAllAsync_CallsService_Always()
        {
            await _websiteStatisticController.GetAllAsync();

            A.CallTo(() => _requestStatisticService.GetAllAsync()).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetAllAsync_ReturnsOkObjectResult_Always()
        {
            var result = await _websiteStatisticController.GetAllAsync();

            result.Should().BeOkObjectResult();
        }

        [Test]
        public async Task GetById_ReturnsNotFount_WhenNotExist()
        {
            A.CallTo(() => _requestStatisticService.GetByIdAsync(Id))
                .ThrowsAsync(new EntityNotFoundException<RequestBenchmarkEntryDto>(Id));

            var result = await _websiteStatisticController.GetByIdAsync(Id);

            result.Should().BeNotFoundResult();
        }

        [Test]
        public async Task GetById_ReturnsOkObjectResult_WhenFound()
        {
            var result = await _websiteStatisticController.GetByIdAsync(Id);

            result.Should().BeOkObjectResult();
        }

        private EvaluatePerformanceRequestModel GetEvaluatePerformanceRequestModel()
        {
            var viewModel = new EvaluatePerformanceRequestModel
            {
                Uri = $"http://{Host}"
            };

            return viewModel;
        }
    }
}
