using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.DAL.Interfaces
{
    public interface IBookingRepository : IGenericRepository<MemberSession>
    {
        IEnumerable<MemberSession> GetAllSessionsById(int sessionId);

        Task<bool> DeleteByEntity(MemberSession entity);
    }
}
