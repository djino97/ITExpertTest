using ExchangeRate.Models;
using ITExpertTestService.Models.Dto;
using ITExpertTestService.Models.Dto.Requests;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTest.Business.Interfaces
{
    public interface IFindObjectsCommand
    {
        Task<OperationResultResponse<FindObjectResponse>> ExecuteAsync(
            FindObjectFilter filter,
            CancellationToken cancellationToken = default);
    }
}
