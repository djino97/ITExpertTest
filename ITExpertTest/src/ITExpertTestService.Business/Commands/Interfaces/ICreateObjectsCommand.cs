using ExchangeRate.Models;
using ITExpertTestService.Models.Dto.NewFolder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITExpertTest.Business.Commands.Interfaces
{
    public interface ICreateObjectsCommand
    {
        Task<OperationResultResponse<IEnumerable<int>>> ExecuteAsync(CreateObjectsRequest request);
    }
}
