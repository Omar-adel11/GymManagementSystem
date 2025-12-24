using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.ModelViews.SessionsModelViews;
using GYM.DAL.Entities;

namespace GYM.BLL.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionModelView>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                    .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                    .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionModelView, Session>();
            CreateMap<UpdateSession, Session>().ReverseMap();

         
        }
    }
}
