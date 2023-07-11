using ITExpertTestService.Data.Providers.Interfaces;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITExpertTestService.Data.Repository
{
    public class ObjectRepository : IObjectRepository
    {
        private readonly IDataProvider _provider;

        public ObjectRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<int>> CreateAsync(IEnumerable<DbObject> dbObjects, CancellationToken cancellationToken = default)
        {
            if (dbObjects == null || !dbObjects.Any())
            {
                return null;
            }

            _provider.Objects.AddRange(dbObjects);
            await _provider.SaveAsync();

            return _provider.Objects.Select(t => t.SerialNumber);
        }

        public async Task<bool> RemoveAllFromTableAsync()
        {
            var obj = _provider.Objects.ToList();
            _provider.Objects.RemoveRange(obj);
            await _provider.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<DbObject>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _provider.Objects.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DbObject>> FindAsync(FindObjectFilter filter, CancellationToken cancellationToken = default)
        {
            if (filter == null)
            {
                return null;
            }

            IQueryable<DbObject> dbObjects = _provider.Objects.AsQueryable();

            if (filter.SerialNumber.HasValue)
            {
                dbObjects = dbObjects.Where(t => t.SerialNumber == filter.SerialNumber.Value);
            }

            if (filter.Code.HasValue)
            {
                dbObjects = dbObjects.Where(t => t.Code == filter.Code.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Value))
            {
                dbObjects = dbObjects.Where(t => t.Value.Contains(filter.Value));
            }

            return await dbObjects
                .Skip(filter.SkipCount)
                .Take(filter.TakeCount)
                .ToListAsync(cancellationToken);
        }
    }
}
