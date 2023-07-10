using ITExpertTestService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ITExpertTestService.Data.Providers.Interfaces
{
    public interface IDataProvider
    {
        DbSet<DbObject> Objects { get; set; }

        void EnsureDeleted();
        bool IsInMemory();
        object MakeEntityDetached(object obj);
        Task SaveAsync();
    }
}
