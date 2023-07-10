using ITExpertTestService.Data.Providers.Interfaces;
using ITExpertTestService.Data.Repository;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Models.Db;
using ITExpertTestService.Models.Dto.Requests;
using ITExpertTestService.Provider.Ef.MsSql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITExpertTestService.Data.UnitTests
{
    internal class ObjectRepositoryTests
    {
        private IDataProvider _provider;
        private IObjectRepository _repository;

        private List<DbObject> _dbObject;

        private DbContextOptions<ITExpertServiceDbContext> _dbContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dbObject = new();
            for (int i = 1; i <= 4; i++)
            {
                _dbObject.Add(
                    new DbObject
                    {
                        Code = 10 * i,
                        Value = "Value10"
                    });
            }

            _dbContext = new DbContextOptionsBuilder<ITExpertServiceDbContext>()
                  .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                  .Options;
        }

        [SetUp]
        public void SetUp()
        {
            _provider = new ITExpertServiceDbContext(_dbContext);
            _repository = new ObjectRepository(_provider);

            _provider.Objects.AddRange(_dbObject);
            _provider.SaveAsync();
        }

        [Test]
        public async Task ShouldCreateSuccessfulAsync()
        {
            List<DbObject> dbObject = new();
            for (int i = 1; i <= 4; i++)
            {
                dbObject.Add(
                    new DbObject
                    {
                        Code = 20 * i,
                        Value = "Value10"
                    });
            }

            var serialNumbers = await _repository.CreateAsync(dbObject);
            var objects = _provider.Objects.ToList();
            foreach (var serialNumber in serialNumbers)
            {
                Assert.IsTrue(objects.Any(x => x.SerialNumber == serialNumber));
            }
        }

        [Test]
        public async Task ShouldGetAllSuccessfulAsync()
        {
            var objects = await _repository.GetAllAsync();
            var expectedObjects = _provider.Objects.ToList();
            foreach (var _object in objects)
            {
                Assert.IsTrue(expectedObjects.Any(x => x.SerialNumber == _object.SerialNumber));
            }
        }

        [Test]
        public async Task ShouldFindByRangeObjectSuccessfulAsync()
        {
            var filter = new FindObjectFilter
            {
                SkipCount = 1,
                TakeCount = 2
            };

            var objects = await _repository.FindAsync(filter);
            var expectedObjects = _provider.Objects
                .Skip(filter.SkipCount)
                .Take(filter.TakeCount)
                .ToList();
            foreach (var _object in objects)
            {
                Assert.IsTrue(expectedObjects.Any(x => x.SerialNumber == _object.SerialNumber));
            }
        }

        [Test]
        public async Task ShouldFindBySerialNumberObjectSuccessfulAsync()
        {
            var filter = new FindObjectFilter
            {
                SerialNumber = _dbObject[2].SerialNumber,
                SkipCount = 1,
                TakeCount = 2
            };

            var objects = await _repository.FindAsync(filter);
            var expectedObjects = _provider.Objects
                .Skip(filter.SkipCount)
                .Take(filter.TakeCount)
                .Where(x => x.SerialNumber == filter.SerialNumber)
                .ToList();
            foreach (var _object in objects)
            {
                Assert.IsTrue(expectedObjects.Any(x => x.SerialNumber == _object.SerialNumber));
            }
        }

        [Test]
        public async Task ShouldFindByCodeObjectSuccessfulAsync()
        {
            var filter = new FindObjectFilter
            {
                Code = _dbObject[2].Code,
                SkipCount = 1,
                TakeCount = 2
            };

            var objects = await _repository.FindAsync(filter);
            var expectedObjects = _provider.Objects
                .Skip(filter.SkipCount)
                .Take(filter.TakeCount)
                .Where(x => x.Code == filter.Code)
                .ToList();
            foreach (var _object in objects)
            {
                Assert.IsTrue(expectedObjects.Any(x => x.SerialNumber == _object.SerialNumber));
            }
        }

        [Test]
        public async Task ShouldFindByValueObjectSuccessfulAsync()
        {
            var filter = new FindObjectFilter
            {
                Value = _dbObject[2].Value,
                SkipCount = 1,
                TakeCount = 2
            };

            var objects = await _repository.FindAsync(filter);
            var expectedObjects = _provider.Objects
                .Skip(filter.SkipCount)
                .Take(filter.TakeCount)
                .Where(x => x.Value.Contains(filter.Value))
                .ToList();
            foreach (var _object in objects)
            {
                Assert.IsTrue(expectedObjects.Any(x => x.SerialNumber == _object.SerialNumber));
            }
        }
    }
}
