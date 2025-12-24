using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;
using GYM.DAL.Repositories;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository(GYMDbContext _context) : GenericRepository<Session>(_context), ISessionRepository
    {
        
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null)
        {
            if (condition is null)
            {
                return _context.Sessions
                    .Include(s => s.Trainer)
                    .Include(s => s.Category)
                    .ToList();
            }
            else
            {
                return _context.Sessions
                    .Include(s => s.Trainer)
                    .Include(s => s.Category)
                    .Where(condition)
                    .ToList();
            }
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _context.MemberSessions.Count(ms => ms.SessionId == sessionId);
        }

        public Session GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _context.Sessions
                    .Include(s => s.Trainer)
                    .Include(s => s.Category)
                    .FirstOrDefault(x => x.Id == sessionId);
        }
    }
}