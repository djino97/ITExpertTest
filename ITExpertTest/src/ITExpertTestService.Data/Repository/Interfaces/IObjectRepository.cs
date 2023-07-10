using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTestService.Data.Repository.Interfaces
{
    public interface IObjectRepository
    {
        Task<IEnumerable<int>> CreateAsync(IEnumerable<DbObject> dbObjects, CancellationToken cancellationToken = default);
        Task<bool> RemoveAllFromTableAsync();
        Task<IEnumerable<DbObject>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<DbObject>> FindAsync(FindObjectFilter filter, CancellationToken cancellationToken = default);
    }
}
