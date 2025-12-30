using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.DAL.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        IEnumerable<Membership> GetAllMembershipsWithMembersAndPlans(Func<Membership, bool>? filter = null);
        Membership? GetFirstOrDefault(Func<Membership, bool>? filter = null);
        Task<bool> DeleteByEntity(Membership entity);
    }
}
