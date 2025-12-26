using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.MemebersModelViews;
using GYM.BLL.ModelViews.TrainersModelView;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class MemberService(IUnitOfWork _unitOfWork, IMapper _mapper) : IMemberService
    {

        public ICollection<MemeberModelView> GetAllMembers()
        {
            var members = _unitOfWork.Repository<Member>().GetAll();
            if(members == null || members.Count == 0)
                return new List<MemeberModelView>();
            else
                return _mapper.Map<ICollection<MemeberModelView>>(members);
        }

        public MemeberModelView? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.Repository<Member>().GetById(memberId);
            if (member == null)
                return null;
            else
            {
                var memberViewModel = _mapper.Map<MemeberModelView>(member);
                
                //active membership
                var activeMembership = _unitOfWork.Repository<Membership>().GetAll(x => x.Id == memberId && x.Status == "Active").FirstOrDefault();
                if (activeMembership != null)
                {
                    memberViewModel.MembershipStartDate = activeMembership.CreateAt.ToString("yyyy-MM-dd");
                    memberViewModel.MembershipEndDate = activeMembership.EndDate.ToString("yyyy-MM-dd");
                }

                //plan name
                var plan = _unitOfWork.PlanRepository().GetById(activeMembership.PlanId);
                if (plan != null)
                {
                    memberViewModel.PlanName = plan.Name;
                }

                return memberViewModel;
            }
        }

        public HealthRecordModelView? GetMemberHealthRecordDetails(int memberId)
        {
            var HR = _unitOfWork.Repository<HealthRecord>().GetById(memberId);
            if (HR is null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<HealthRecordModelView>(HR);
            }
        }

        public async Task<bool> CreateMember(CreateMemberModelView memberModelView)
        {
            if (checkEmailExistence(memberModelView.Email) || checkPhoneExistence(memberModelView.PhoneNumber))
            {
                return false;
            }

            var Member = _mapper.Map<Member>(memberModelView);

            _unitOfWork.Repository<Member>().Add(Member);
            return await _unitOfWork.SaveChangesAsync()>0;
        }


        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.Repository<Member>().GetById(id);
            if (member == null)
                return null;
            else
            {
                return _mapper.Map<MemberToUpdateViewModel>(member);
            }


        }
        public async Task<bool> UpdateMember(int id, MemberToUpdateViewModel memberModelView)
        {
            try
            {
                var member = _unitOfWork.Repository<Member>().GetById(id);
                if (member is null || checkEmailExistence(memberModelView.Email) || checkPhoneExistence(memberModelView.PhoneNumber)) return false;
                else
                {
                    _mapper.Map(memberModelView, member);
                }
                _unitOfWork.Repository<Member>().Update(member);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch 
            {
                return false;
            }
           

        }

        public async Task<bool> RemoveMember(int id)
        {
            var member = _unitOfWork.Repository<Member>().GetById(id);
            if (member is not null)
            {
                _unitOfWork.Repository<Member>().Delete(id);
            }
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


        private bool checkEmailExistence(string Email)
        {
            var isExist = _unitOfWork.Repository<Member>().GetAll(m => m.Email == Email).Any();
            return isExist;
        }

        private bool checkPhoneExistence(string phone)
        {
            var isExist = _unitOfWork.Repository<Member>().GetAll(m => m.phone == phone).Any();
            return isExist;
        }
    }
}
