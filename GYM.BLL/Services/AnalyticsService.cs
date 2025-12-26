using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.AnalyticsViewModel;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class AnalyticsService(IUnitOfWork _unitOfWork) : IAnalyticsService
    {
        public AnalyticsViewModel GetAllAnalytics()
        {
            var sessions = _unitOfWork.Repository<Session>().GetAll();

            return new AnalyticsViewModel()
            {
                TotalMembers = _unitOfWork.Repository<Member>().GetAll().Count(),
                ActiveMembers = _unitOfWork.Repository<Membership>().GetAll(m=>m.Status == "Active").Count(),
                TotalTrainers = _unitOfWork.Repository<Trainer>().GetAll().Count(),
                UpcomingSessions = sessions.Where(s => s.StartDate > DateTime.UtcNow).Count(),
                CompletedSessions = sessions.Where(s => s.EndDate < DateTime.UtcNow).Count(),
                OngoingSessions = sessions.Where(s=>s.StartDate <= DateTime.UtcNow && s.EndDate > DateTime.Now).Count(),

            };
        }
    }
}
