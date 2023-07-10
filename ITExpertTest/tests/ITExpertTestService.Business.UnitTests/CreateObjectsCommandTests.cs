using ExchangeRate.Models;
using ITExpertTest.Business.Commands;
using ITExpertTest.Business.Commands.Interfaces;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.NewFolder;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTestService.Business.UnitTests
{
    internal class CreateObjectsCommandTests
    {
        private Mock<IObjectRepository> _repository;
        private Mock<IMapperCreateObjectRequest> _mapper;
        private ICreateObjectsCommand _createObjectsCommand;

        private CreateObjectsRequest _request;
        private HttpContextAccessor _httpContextAccessor;
        private List<DbObject> _dbObjects;
        private List<ObjectRequest> _objectRequests;
        private OperationResultResponse<IEnumerable<int>> _expectedResponse;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IObjectRepository>();
            _mapper = new Mock<IMapperCreateObjectRequest>();
            _httpContextAccessor = new HttpContextAccessor();
            _httpContextAccessor.HttpContext = new DefaultHttpContext();

            _createObjectsCommand =
                new CreateObjectsCommand(
                _repository.Object,
                _mapper.Object,
                _httpContextAccessor);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _objectRequests = new();
            for (int i = 1; i <= 4; i++)
            {
                _objectRequests.Add(
                    new ObjectRequest
                    {
                        Code = 10 * i,
                        Value = "Value10"
                    });
            }

            _dbObjects = new();
            for (int i = 1; i <= _objectRequests.Count; i++)
            {
                _dbObjects.Add(
                    new DbObject
                    {
                        SerialNumber = i,
                        Code = _objectRequests[i - 1].Code,
                        Value = _objectRequests[i - 1].Value
                    });
            }

            _request = new CreateObjectsRequest
            {
                Objects = _objectRequests
            };
            _expectedResponse = new();
            _expectedResponse.Result = _dbObjects.Select(x => x.SerialNumber);
        }

        [Test]
        public async Task ShouldReturnErrorWhenRequestIsNullAsync()
        {
            var request = new CreateObjectsRequest
            {
                Objects = null
            };

            var response = await _createObjectsCommand.ExecuteAsync(request);
            Assert.IsNotNull(response.Error);
            Assert.IsNull(response.Result);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task ShouldReturnErrorWhenRequestIsEmptyAsync()
        {
            var request = new CreateObjectsRequest
            {
                Objects = new List<ObjectRequest>()
            };

            var response = await _createObjectsCommand.ExecuteAsync(request);
            Assert.IsNotNull(response.Error);
            Assert.IsNull(response.Result);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status400BadRequest);
        }

        [Test]
        public async Task ShouldReturnErrorWhenRepositoryThrewExceptionAsync()
        {
            _mapper
                .SetupSequence(x => x.Map(It.IsAny<ObjectRequest>()))
                .Returns(_dbObjects[0])
                .Returns(_dbObjects[2])
                .Returns(_dbObjects[3])
                .Returns(_dbObjects[1]);

            _repository
                .Setup(x =>
                    x.CreateAsync(_dbObjects,
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var response = await _createObjectsCommand.ExecuteAsync(_request);

            Assert.IsNotNull(response.Error);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status503ServiceUnavailable);
        }

        [Test]
        public async Task ShouldCreateObjectsAndReturnSerialNumbersAsync()
        {
            _mapper
                .SetupSequence(x => x.Map(It.IsAny<ObjectRequest>()))
                .Returns(_dbObjects[0])
                .Returns(_dbObjects[2])
                .Returns(_dbObjects[3])
                .Returns(_dbObjects[1]);

            _repository
                .Setup(x =>
                    x.CreateAsync(_dbObjects,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_dbObjects.Select(x => x.SerialNumber));

            var response = await _createObjectsCommand.ExecuteAsync(_request);

            for (int i = 0; i < _objectRequests.Count; i++)
            {
                Assert.AreEqual(_expectedResponse.Result.ElementAt(i), response.Result.ElementAt(i));
            }
            Assert.IsNull(response.Error);
            Assert.AreEqual(_httpContextAccessor.HttpContext.Response.StatusCode, StatusCodes.Status200OK);
        }
    }
}