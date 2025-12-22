using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GYM.DAL.Repositories
{
    public class GenericRepository<T>(GYMDbContext _context) : IGenericRepository<T> where T : BaseEntity, new()
    {
        public ICollection<T> GetAll(Func<T, bool>? Condition = null)
        {
            if(Condition == null)
                return _context.Set<T>().AsNoTracking().ToList();
            else
                return _context.Set<T>().Where(Condition).ToList();
        }

        public T? GetById(int id) => _context.Set<T>().Find(id);
      
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            _context.Set<T>().Remove(entity);
        }

    }
}
