using ExchangeRate.Models;
using ITExpertTest.Business.Commands.Interfaces;
using ITExpertTest.Business.Interfaces;
using ITExpertTestService.Models.Dto;
using ITExpertTestService.Models.Dto.NewFolder;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ObjectsController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<OperationResultResponse<IEnumerable<int>>> CreateAsync(
            [FromServices] ICreateObjectsCommand command,
            [FromBody] CreateObjectsRequest request)
        {
            return await command.ExecuteAsync(request);
        }

        [HttpGet("find")]
        public async Task<OperationResultResponse<FindObjectResponse>> FindAsync(
            [FromServices] IFindObjectsCommand command,
            [FromQuery] FindObjectFilter filter,
            CancellationToken cancellationToken)
        {
            return await command.ExecuteAsync(filter, cancellationToken);
        }
    }
}
