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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GYMDbContext _context;

        public MembershipRepository(GYMDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans(Func<Membership, bool>? filter = null)
        {
            var memberships = _context.Memberships.Include(m => m.Member).Include(m => m.Plan)
                            .Where(filter ?? (_ => true));

            return memberships;

        }

        public Membership? GetFirstOrDefault(Func<Membership, bool>? filter = null)
        {
            var membership = _context.Memberships.Include(m => m.Member).Include(m => m.Plan)
                            .FirstOrDefault(filter ?? (_ => true));
            return membership;
        }

        public async Task<bool> DeleteByEntity(Membership entity)
        {
             _context.Memberships.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
