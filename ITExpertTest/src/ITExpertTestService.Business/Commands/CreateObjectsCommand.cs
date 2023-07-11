using ExchangeRate.Models;
using ITExpertTest.Business.Commands.Interfaces;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.NewFolder;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITExpertTest.Business.Commands
{
    public class CreateObjectsCommand : ICreateObjectsCommand
    {
        private readonly IMapperCreateObjectRequest _mapper;
        private readonly IObjectRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private void ValidateRequest(CreateObjectsRequest request, List<string> errors)
        {
            if (request.Objects == null || !request.Objects.Any())
            {
                errors.Add("Request must contain at least one object.");
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        public CreateObjectsCommand(
            IObjectRepository repository,
            IMapperCreateObjectRequest mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResultResponse<IEnumerable<int>>> ExecuteAsync(CreateObjectsRequest request)
        {
            List<string> errors = new();
            ValidateRequest(request, errors);

            if (errors.Any())
            {
                return new OperationResultResponse<IEnumerable<int>>(error: errors);
            }

            List<DbObject> dbObjects = new();
            foreach (ObjectRequest objectInfo in request.Objects)
            {
                dbObjects.Add(_mapper.Map(objectInfo));
            }

            dbObjects = dbObjects.OrderBy(x => x.Code).ToList();

            try
            {
                await _repository.RemoveAllFromTableAsync();
                IEnumerable<int> serialNumbers = await _repository.CreateAsync(dbObjects);
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                return new OperationResultResponse<IEnumerable<int>>(serialNumbers);
            }
            catch (Exception exc)
            {
                errors.Add("Something went wrong. Please try again later.");
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            }

            return new OperationResultResponse<IEnumerable<int>>(error: errors);
        }
    }
}
