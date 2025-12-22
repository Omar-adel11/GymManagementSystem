using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        ICollection<T> GetAll(Func<T,bool>? Condition = null);

        T? GetById(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(int id);


    }
}
