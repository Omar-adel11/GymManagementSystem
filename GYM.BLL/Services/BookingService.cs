using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.BookingModelViews;
using GYM.BLL.ModelViews.MembershipsViewModel;
using GYM.BLL.ModelViews.SessionsModelViews;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class BookingService(IUnitOfWork _unitOfWork, IMapper _mapper) : IBookingService
    {
        public IEnumerable<MemberForSessionViewModel> GetAllMembersBySessionId(int sessionId)
        {
            var memberSessions = _unitOfWork.bookingRepository.GetAllSessionsById(sessionId);
            var mappedMembers = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(memberSessions);
            return mappedMembers;
        }

        public IEnumerable<SessionModelView> GetAllSessionsWithTrainerAndCategory()
        {
            var sessions = _unitOfWork.sessionRepository.GetAllSessionsWithTrainerAndCategory();

            var sessionsModelView = _mapper.Map<IEnumerable<SessionModelView>>(sessions);

            foreach(var session in sessionsModelView)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id);
            }

            return sessionsModelView;

        }

        public async Task<bool> Create(CreateBookingViewModel model)
        {
            try
            {
                var session = _unitOfWork.sessionRepository.GetById(model.SessionId);
                if (session is null || session.StartDate <= DateTime.UtcNow)
                    return false;

                
                var membershipRepo = _unitOfWork.MembershipRepository();
                var activeMembership = membershipRepo.GetFirstOrDefault(m => m.MemberId == model.MemberId && m.Status == "Active");

                if (activeMembership is null)
                    return false;

                

                var sessionRepo = _unitOfWork.sessionRepository;
                var bookedSlots = sessionRepo.GetCountOfBookedSlots(model.SessionId);

                var availableSlots = session.Capacity - bookedSlots;
                if (availableSlots == 0)
                    return false;

                var booking = _mapper.Map<MemberSession>(model);
               

                booking.IsAttended = false;
                _unitOfWork.bookingRepository.Add(booking);


                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Cancel(MemberForSessionViewModel model)
        {
            try
            {
                var session = _unitOfWork.sessionRepository.GetById(model.SessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var Booking = _unitOfWork.bookingRepository.GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                                           .FirstOrDefault();
                if (Booking is null) return false;
                await _unitOfWork.bookingRepository.DeleteByEntity(Booking);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }


        }

        public async Task<bool> Attended(MemberForSessionViewModel model)
        {
            try
            {
                var memberSession = _unitOfWork.Repository<MemberSession>()
                                           .GetAll(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                           .FirstOrDefault();
                if (memberSession is null) return false;

                memberSession.IsAttended = true;
                memberSession.UpdateAt = DateTime.Now;
                _unitOfWork.Repository<MemberSession>().Update(memberSession);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<MemberSelectListViewModel> GetMembersForDropdown(int id)
        {
            // Get Members who already booked this session
            var bookingRepo = _unitOfWork.bookingRepository;
            var bookedMemberIds = bookingRepo.GetAll(s => s.Id == id)
                                                      .Select(s => s.MemberId)
                                                      .ToList();

            var availableMembersToBook = _unitOfWork.Repository<Member>().GetAll(m => !bookedMemberIds.Contains(m.Id));

            var memberSelectListViewModel = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembersToBook);

            return memberSelectListViewModel;

        }

       
    }
}
