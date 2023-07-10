using ITExpertTestService.Data.Providers.Interfaces;
using ITExpertTestService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;

namespace ITExpertTestService.Provider.Ef.MsSql
{
    public class ITExpertServiceDbContext : DbContext, IDataProvider
    {
        public ITExpertServiceDbContext(DbContextOptions<ITExpertServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbObject> Objects { get; set; }

        // Fluent API is written here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("ITExpertTestService.Models.Db"));
        }

        public object MakeEntityDetached(object obj)
        {
            Entry(obj).State = EntityState.Detached;

            return Entry(obj).State;
        }

        public void EnsureDeleted()
        {
            Database.EnsureDeleted();
        }

        public bool IsInMemory()
        {
            return Database.IsInMemory();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }
    }
}
