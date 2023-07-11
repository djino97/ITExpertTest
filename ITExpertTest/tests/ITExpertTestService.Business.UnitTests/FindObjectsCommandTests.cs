using ExchangeRate.Models;
using ITExpertTest.Business.Commands;
using ITExpertTest.Business.Interfaces;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTestService.Business.UnitTests
{
    internal class FindObjectsCommandTests
    {
        private Mock<IObjectRepository> _repository;
        private Mock<IMapperFindObjectResponse> _mapper;
        private Mock<ILogger<FindObjectsCommand>> _logger;
        private IFindObjectsCommand _findObjectsCommand;

        private FindObjectFilter _request;
        private HttpContextAccessor _httpContextAccessor;
        private List<DbObject> _dbObjects;
        private List<ObjectInfo> _objectsInfo;
        private OperationResultResponse<FindObjectResponse> _expectedResponse;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IObjectRepository>();
            _mapper = new Mock<IMapperFindObjectResponse>();
            _logger = new Mock<ILogger<FindObjectsCommand>>();
            _httpContextAccessor = new HttpContextAccessor();
            _httpContextAccessor.HttpContext = new DefaultHttpContext();

            _findObjectsCommand =
                new FindObjectsCommand(
                _repository.Object,
                _mapper.Object,
                _logger.Object,
                _httpContextAccessor);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dbObjects = new();
            for (int i = 1; i <= 4; i++)
            {
                _dbObjects.Add(
                    new DbObject
                    {
                        SerialNumber = i,
                        Code = i * 10,
                        Value = $"Value{i * 10}"
                    });
            }

            _objectsInfo = new();
            foreach (var dbObject in _dbObjects)
            {
                _objectsInfo.Add(
                    new ObjectInfo
                    {
                        SerialNumber = dbObject.SerialNumber,
                        Code = dbObject.Code,
                        Value = dbObject.Value
                    });
            }

            _expectedResponse = new();
            _expectedResponse.Result = new FindObjectResponse();
        }

        [Test]
        public async Task ShouldReturnErrorWhenRequestWasNotValidAsync()
        {
            var request = new FindObjectResponse
            {
                Objects = null
            };

            _request = new FindObjectFilter
            {
                SkipCount = -1,
                TakeCount = 3
            };

            var response = await _findObjectsCommand.ExecuteAsync(_request);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task ShouldReturnErrorWhenRepositoryThrewExceptionAsync()
        {
            var request = new FindObjectResponse
            {
                Objects = null
            };

            _request = new FindObjectFilter
            {
                SkipCount = 0,
                TakeCount = 3
            };

            _expectedResponse.Result.Objects = _objectsInfo;
            _expectedResponse.Result.TotalCount = _objectsInfo.Count;

            _repository
                .Setup(x =>
                    x.FindAsync(_request,
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var response = await _findObjectsCommand.ExecuteAsync(_request);
            Assert.IsTrue(response.Error.Any());
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status503ServiceUnavailable);
        }

        [Test]
        public async Task ShouldSuccessfulReturnRangeObjectsResponseAsync()
        {
            var request = new FindObjectResponse
            {
                Objects = null
            };

            _request = new FindObjectFilter
            {
                SkipCount = 0,
                TakeCount = 3
            };

            _expectedResponse.Result.Objects = _objectsInfo;
            _expectedResponse.Result.TotalCount = _objectsInfo.Count;

            _repository
                .Setup(x =>
                    x.FindAsync(_request,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_dbObjects);

            _repository
                .Setup(x =>
                    x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_dbObjects);

            _mapper
                .SetupSequence(x => x.Map(It.IsAny<DbObject>()))
                .Returns(_objectsInfo[0])
                .Returns(_objectsInfo[1])
                .Returns(_objectsInfo[2])
                .Returns(_objectsInfo[3]);

            var response = await _findObjectsCommand.ExecuteAsync(_request);
            for (int i = 0; i < response.Result.Objects.Count; i++)
            {
                Assert.AreEqual(_expectedResponse.Result.Objects[i], response.Result.Objects[i]);
            }
            Assert.IsNull(response.Error);
            Assert.AreEqual(_expectedResponse.Result.TotalCount, response.Result.TotalCount);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status200OK);
        }
    }
}
