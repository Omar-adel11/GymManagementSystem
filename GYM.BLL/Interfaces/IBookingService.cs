using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.BookingModelViews;
using GYM.BLL.ModelViews.MembershipsViewModel;
using GYM.BLL.ModelViews.SessionsModelViews;

namespace GYM.BLL.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionModelView> GetAllSessionsWithTrainerAndCategory();

        IEnumerable<MemberForSessionViewModel> GetAllMembersBySessionId(int sessionId);

        Task<bool> Create(CreateBookingViewModel model);
        IEnumerable<MemberSelectListViewModel> GetMembersForDropdown(int id);

        Task<bool> Cancel(MemberForSessionViewModel model);
        Task<bool> Attended(MemberForSessionViewModel model);



    }
}
