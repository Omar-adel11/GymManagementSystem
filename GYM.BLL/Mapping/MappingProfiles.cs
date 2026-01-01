using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using GYM.BLL.ModelViews.BookingModelViews;
using GYM.BLL.ModelViews.MembershipsViewModel;
using GYM.BLL.ModelViews.MemebersModelViews;
using GYM.BLL.ModelViews.PlansModelViews;
using GYM.BLL.ModelViews.SessionsModelViews;
using GYM.BLL.ModelViews.TrainersModelView;
using GYM.DAL.Entities;

namespace GYM.BLL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Session
            CreateMap<Session, SessionModelView>()
                        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                        .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                        .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionModelView, Session>();
            CreateMap<UpdateSession, Session>().ReverseMap();

            CreateMap<Category, CategorySelectViewModel>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
            CreateMap<Trainer, TrainerSelectViewModel>();
            #endregion

            #region Member
            CreateMap<DAL.Entities.Member, MemeberModelView>()
                                                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                                                 .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.phone))
                                                 .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                                                 .ForMember(dest => dest.Address,opt => opt.MapFrom(src => $"{src.Address.BuildingNo}, {src.Address.Street}, {src.Address.City}"));


            CreateMap<HealthRecord, HealthRecordModelView>().ReverseMap();

            CreateMap<CreateMemberModelView, DAL.Entities.Member>()
                                                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.PhoneNumber))
                                                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                                                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecord));
                                                
            CreateMap<CreateMemberModelView, Address>()
                                                .ForMember(dest => dest.Street, opt => opt.MapFrom(src=>src.Street))
                                                .ForMember(dest => dest.City, opt => opt.MapFrom(src=>src.City))
                                                .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src=>src.BuildingNo));

            CreateMap<DAL.Entities.Member, MemberToUpdateViewModel>()
                                                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.phone))
                                                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                                                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                                                .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.Address.BuildingNo));

            CreateMap<MemberToUpdateViewModel, DAL.Entities.Member>()
                                                .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.PhoneNumber))
                                                .ForMember(dest => dest.Name, opt => opt.Ignore())
                                                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                                                .AfterMap((src, dest) =>
                                                {
                                                    dest.Address.Street = src.Street;
                                                    dest.Address.City = src.City;
                                                    dest.Address.BuildingNo = src.BuildingNo;
                                                    
                                                });



            #endregion

            #region Plan
            CreateMap<Plan, PlanModelView>();
            CreateMap<PlanToUpdateModelView, Plan>().ForMember(dest => dest.Name,opt => opt.MapFrom(src => src.PlanName));
            CreateMap<Plan, PlanToUpdateModelView>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
            {
                src.Price = dest.Price;
                src.Name = dest.PlanName;
                src.Description = dest.Description;
                src.DurationDays = dest.DurationDays;
                src.UpdateAt = DateTime.Now;

            });


            #endregion

            #region Trainer
            CreateMap<Trainer, TrainerModelView>().ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                                                  .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                                                  .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.Address.BuildingNo))
                                                  .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialities.ToString()));

            CreateMap<TrainerToBeCreatedModelView, Trainer>().ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                                                  .ForMember(dest => dest.Specialities, opt => opt.MapFrom(src => src.Specialization));

            CreateMap<TrainerToBeCreatedModelView, Address>().ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                                                  .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                                                  .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.BuildingNo));

            CreateMap<Trainer, TrainerToBeUpdatedModelView>().ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                                                  .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                                                  .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialities))
                                                  .ForMember(dest => dest.BuildingNo, opt => opt.MapFrom(src => src.Address.BuildingNo));

            CreateMap<TrainerToBeUpdatedModelView, Trainer>().AfterMap((src, dest) =>
            {
                dest.Email = src.Email;
                dest.phone = src.Phone;
                dest.Specialities = src.Specialization;
                dest.Address.Street = src.Street;
                dest.Address.City = src.City;
                dest.Address.BuildingNo = src.BuildingNo;

                dest.UpdateAt = DateTime.Now;

            });
            #endregion

            #region Membership
            CreateMap<Membership, MembershipViewModel>().ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                                                       .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                                                       .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreateAt));
            CreateMap<DAL.Entities.Member, MemberSelectListViewModel>();
            CreateMap<DAL.Entities.Plan, PlanSelectListViewModel>();

             

            CreateMap<CreateMembershipModelView, Membership>();
            #endregion

            #region MemberSessions
            CreateMap<MemberSession,MemberForSessionViewModel>().ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                                                        .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreateAt.ToString()));
           
            CreateMap<CreateBookingViewModel, MemberSession>();
            #endregion
        }
    }
}
