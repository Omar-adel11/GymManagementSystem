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
    public class BookingRepository : GenericRepository<MemberSession>, IBookingRepository
    {
        private readonly GYMDbContext _context;

        public BookingRepository( GYMDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DeleteByEntity(MemberSession entity)
        {
            _context.MemberSessions.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<MemberSession> GetAllSessionsById(int sessionId)
        {
            var sessions = _context.MemberSessions.Where(ms => ms.SessionId == sessionId)
                                                  .Include(ms => ms.Member)
                                                  .ToList();
            return sessions;
        }
    }
}
