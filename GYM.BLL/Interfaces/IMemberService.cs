using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.MemebersModelViews;
using GYM.DAL.Entities;

namespace GYM.BLL.Interfaces
{
    public interface IMemberService
    {
        ICollection<MemeberModelView> GetAllMembers();
        MemeberModelView? GetMemberDetails(int memberId);
        HealthRecordModelView? GetMemberHealthRecordDetails(int memberId);
        Task<bool> CreateMember(CreateMemberModelView memberModelView);
        MemberToUpdateViewModel? GetMemberToUpdate(int id);
        Task<bool> UpdateMember(int id,MemberToUpdateViewModel memberModelView);
        Task<bool> RemoveMember(int id);
    }
}
