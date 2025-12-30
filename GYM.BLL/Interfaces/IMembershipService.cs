using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.MembershipsViewModel;
using GYM.DAL.Entities;

namespace GYM.BLL.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetAllMemberships();
        IEnumerable<PlanSelectListViewModel> GetPlansForDropDown();
        IEnumerable<MemberSelectListViewModel> GetMembersForDropDown();
        Task<bool> CreateMembership(CreateMembershipModelView membershipModel);
        Task<bool> DeleteMembership(int memberId);
    }
}
