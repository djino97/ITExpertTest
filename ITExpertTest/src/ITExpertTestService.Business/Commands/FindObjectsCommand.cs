using ExchangeRate.Models;
using ITExpertTest.Business.Interfaces;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTest.Business.Commands
{
    public class FindObjectsCommand : IFindObjectsCommand
    {
        private readonly IMapperFindObjectResponse _mapper;
        private readonly IObjectRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private void ValidateRequest(FindObjectFilter filter, List<string> errors)
        {
            if (filter.SkipCount < 0)
            {
                errors.Add("SkipCount must not be less than zero.");
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            if (filter.TakeCount <= 0)
            {
                errors.Add("TakeCount must not be less than zero or equal to zero.");
            }

            if (errors.Any())
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        public FindObjectsCommand(
            IObjectRepository repository,
            IMapperFindObjectResponse mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResultResponse<FindObjectResponse>> ExecuteAsync(
            FindObjectFilter filter,
            CancellationToken cancellationToken = default)
        {
            List<string> errors = new();
            ValidateRequest(filter, errors);
            if (errors.Any())
            {
                return new OperationResultResponse<FindObjectResponse>(error: errors);
            }

            FindObjectResponse findObjectResponse = new();
            IEnumerable<DbObject> dbObjects = new List<DbObject>();
            try
            {
                dbObjects = await _repository.FindAsync(filter, cancellationToken);
                findObjectResponse.TotalCount = (await _repository.GetAllAsync(cancellationToken)).Count();
            }
            catch (Exception exc)
            {
                errors.Add("Something went wrong. Please try again later.");
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new OperationResultResponse<FindObjectResponse>(error: errors);
            }

            findObjectResponse.Objects = new();
            foreach (DbObject dbObject in dbObjects)
            {
                findObjectResponse.Objects.Add(_mapper.Map(dbObject));
            }

            return new OperationResultResponse<FindObjectResponse>(findObjectResponse);
        }
    }
}
