using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.MemebersModelViews;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class MemberService(IUnitOfWork _unitOfWork) : IMemberService
    {

        public ICollection<MemeberModelView> GetAllMembers()
        {
            var members = _unitOfWork.Repository<Member>().GetAll();
            if(members == null || members.Count == 0)
                return new List<MemeberModelView>();
            else
                return members.Select(m => new MemeberModelView
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    PhoneNumber = m.phone,
                    Gender = m.Gender.ToString(),
                    Photo = m.Photo
                }).ToList();
        }

        public MemeberModelView? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.Repository<Member>().GetById(memberId);
            if (member == null)
                return null;
            else
            {
                MemeberModelView memberViewModel = new MemeberModelView()
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    PhoneNumber = member.phone,
                    Gender = member.Gender.ToString(),
                    Photo = member.Photo,
                    BirthDate = member.DateOfBirth.ToString("yyyy-MM-dd"),
                    Address = $"{member.Address.BuildingNo}, {member.Address.Street}, {member.Address.City}",

                };

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
                return new HealthRecordModelView
                {
                    Height = HR.Height,
                    Weight = HR.Weight,
                    BloodType = HR.BloodType,
                    Note = HR.Note
                };
            }
        }

        public async Task<bool> CreateMember(CreateMemberModelView memberModelView)
        {
            if (checkEmailExistence(memberModelView.Email) || checkPhoneExistence(memberModelView.PhoneNumber))
            {
                return false;
            }

            var Member = new Member()
            {
                Name = memberModelView.Name,
                Email = memberModelView.Email,
                phone = memberModelView.PhoneNumber,
                Photo = memberModelView.Photo,
                Gender = memberModelView.Gender,
                Address = new Address
                {
                    BuildingNo = memberModelView.BuildingNo,
                    Street = memberModelView.Street,
                    City = memberModelView.City
                },
                DateOfBirth = memberModelView.DateOfBirth,
                HealthRecord = new HealthRecord
                {
                    Height = memberModelView.HealthRecord!.Height,
                    Weight = memberModelView.HealthRecord.Weight,
                    BloodType = memberModelView.HealthRecord.BloodType,
                    Note = memberModelView.HealthRecord.Note
                }
                
            };

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
                return new MemberToUpdateViewModel()
                {
                    Photo = member.Photo,
                    Name = member.Name,
                    Email = member.Email,
                    PhoneNumber = member.phone,
                    BuildingNo = member.Address.BuildingNo,
                    Street = member.Address.Street,
                    City = member.Address.City
                };
            }


        }
        public async Task<bool> UpdateMember(int id, MemberToUpdateViewModel memberModelView)
        {
            var member = _unitOfWork.Repository<Member>().GetById(id);
            if (member is not null)
            {
                member.Email = memberModelView.Email;
                member.phone = memberModelView.PhoneNumber;
                member.Address.Street = memberModelView.Street;
                member.Address.City = memberModelView.City;
                member.Address.BuildingNo = memberModelView.BuildingNo;
            }

            return await _unitOfWork.SaveChangesAsync() > 0;

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
