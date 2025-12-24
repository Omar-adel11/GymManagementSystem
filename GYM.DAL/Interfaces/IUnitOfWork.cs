using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GYM.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity, new();
        ISessionRepository sessionRepository { get; }
        IPlanRepository PlanRepository();
        Task<int> SaveChangesAsync();
    }
}
