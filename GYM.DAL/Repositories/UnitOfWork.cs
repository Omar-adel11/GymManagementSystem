using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.DAL.Repositories
{
    public class UnitOfWork(GYMDbContext _context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repositories = new();

        public IPlanRepository PlanRepository()
        {
            _repositories.TryGetValue(nameof(PlanRepository), out var repositoryObj);
            if (repositoryObj == null)
            {
                var repositoryInstance = new PlanRepository(_context);
                _repositories.TryAdd(nameof(PlanRepository), repositoryInstance);
                return repositoryInstance;
            }
            else
            {
                return (IPlanRepository)repositoryObj;
            }
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new()
        {
            _repositories.TryGetValue(typeof(TEntity).Name, out var repositoryObj);
            if (repositoryObj == null)
            {
                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repositories.TryAdd(typeof(TEntity).Name, repositoryInstance);
                return repositoryInstance;
            }
            else
            {
                return (IGenericRepository<TEntity>)repositoryObj;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
