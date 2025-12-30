using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.MembershipsViewModel;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class MembershipService(IUnitOfWork _unitOfWork,IMapper _mapper) : IMembershipService
    {
        public IEnumerable<MembershipViewModel> GetAllMemberships()
        {
            var memberships = _unitOfWork.MembershipRepository().GetAllMembershipsWithMembersAndPlans(m => m.Status == "Active");
            var membershipViewModels = _mapper.Map<IEnumerable<MembershipViewModel>>(memberships);

            return membershipViewModels;

        }

        public async Task<bool> CreateMembership(CreateMembershipModelView model)
        {
            if (!IsMemberExists(model.MemberId) || !IsPlanExists(model.PlanId) || HasActiveMembership(model.MemberId))
                return false;
            var plan = _unitOfWork.Repository<Plan>().GetById(model.PlanId);
            var member = _unitOfWork.Repository<Member>().GetById(model.MemberId);

            var membershipRepo = _unitOfWork.Repository<Membership>();

            var membership = new Membership
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                Plan = plan,
                Member = member,
                CreateAt = DateTime.Now,
                EndDate = DateTime.Now.AddDays(plan.DurationDays),
                
            };



            // BUSSINESS RULE #5: When a membership is created, its EndDate
            // is automatically calculated based on the plan duration.
           

            membershipRepo.Add(membership);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // BUSSINESS RULE #7: Cancellation Delete Memberships For Member On This Plan 
        // BUSSINESS RULE #8: A membership can only be deleted if it is Active.
        public async Task<bool> DeleteMembership(int MemberId)
        {
            var membershipRepo = _unitOfWork.MembershipRepository();
            var membershipToDelete = membershipRepo.GetFirstOrDefault(m => m.MemberId == MemberId && m.Status == "Active");

            if (membershipToDelete is null)
                return false;

            await membershipRepo.DeleteByEntity(membershipToDelete);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public IEnumerable<PlanSelectListViewModel> GetPlansForDropDown()
        {
            var Plans = _unitOfWork.Repository<Plan>().GetAll(X => X.IsActive == true);
            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(Plans);
        }
        public IEnumerable<MemberSelectListViewModel> GetMembersForDropDown()
        {
            var Members = _unitOfWork.Repository<Member>().GetAll();
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(Members);
        }


        #region Helper methods


        // BUSSINESS RULE #1: A membership can only be created if the member exists in the system
        private bool IsMemberExists(int memberId)
            => _unitOfWork.Repository<Member>().GetById(memberId) is not null;

        // BUSSINESS RULE #2: A membership can only be created if the plan exists in the system.
        private bool IsPlanExists(int planId)
            => _unitOfWork.Repository<Plan>().GetById(planId) is not null;
        // BUSSINESS RULE #3: A member cannot have more than one Active membership at the same time.
        private bool HasActiveMembership(int memberId)
        => _unitOfWork.MembershipRepository().GetAllMembershipsWithMembersAndPlans(m => m.Status == "Active" && m.MemberId == memberId).Any();

        #endregion
    }
}
