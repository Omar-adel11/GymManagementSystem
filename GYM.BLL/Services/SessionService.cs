using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.SessionsModelViews;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    
        public class SessionService : ISessionService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<bool> CreateSession(CreateSessionModelView input)
            {
                if (!IsTrainerExist(input.TrainerId)
                    || !IsCategoryExist(input.CategoryId)
                    || !IsValidDateRange(input.StartDate, input.EndDate))
                { return false; }

                var session = _mapper.Map<CreateSessionModelView, Session>(input);
                _unitOfWork.Repository<Session>().Add(session);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            public IEnumerable<SessionModelView> GetAllSessions()
            {
                var sessions = _unitOfWork.sessionRepository
                                .GetAllSessionsWithTrainerAndCategory()
                                .OrderByDescending(x => x.StartDate);

                if (sessions is null || !sessions.Any())
                    return [];

                var mappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionModelView>>(sessions);

                foreach (var session in mappedSessions)
                    session.AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id);

                return mappedSessions;
            }

            public SessionModelView? GetSessionById(int sessionId)
            {
                var session = _unitOfWork.sessionRepository.GetSessionWithTrainerAndCategory(sessionId);

                if (session is null) return null;

                var mappedSession = _mapper.Map<Session, SessionModelView>(session);
                mappedSession.AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(sessionId);

                return mappedSession;


            }

            public UpdateSession? GetSessionToUpdate(int sessionId)
            {
                var session = _unitOfWork.Repository<Session>().GetById(sessionId);
                if (session is null) return null;

                return _mapper.Map<UpdateSession>(session);
            }

            public async Task<bool> UpdateSession(int sessionId, UpdateSession input)
            {
                var session = _unitOfWork.Repository<Session>().GetById(sessionId);

                if (!IsSessionAvailableForUpdate(session)) return false;
                if (!IsTrainerExist(input.TrainerId)) return false;
                if (!IsValidDateRange(input.StartDate, input.EndDate)) return false;

                session.TrainerId = input.TrainerId;
                session.StartDate = input.StartDate;
                session.EndDate = input.EndDate;
                session.Description = input.Description;
                session.UpdateAt = DateTime.UtcNow;

                _unitOfWork.Repository<Session>().Update(session);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            public async Task<bool> RemoveSession(int sessionId)
            {
                var session = _unitOfWork.Repository<Session>().GetById(sessionId);

                if (!IsSessionAvailableForRemove(session))
                    return false;

                _unitOfWork.Repository<Session>().Delete(session.Id);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }


            #region Helper Methods
            private bool IsTrainerExist(int trainerId)
            {
                var trainer = _unitOfWork.Repository<Trainer>().GetById(trainerId);
                return trainer is null ? false : true;
            }
            private bool IsCategoryExist(int categoryId)
            {
                var category = _unitOfWork.Repository<Category>().GetById(categoryId);
                return category is null ? false : true;
            }
            private bool IsValidDateRange(DateTime startDate, DateTime endDate)
            {
                return endDate > startDate && startDate > DateTime.UtcNow;
            }
            private bool IsSessionAvailableForUpdate(Session session)
            {
                if (session is null) return false;
                if (session.EndDate < DateTime.UtcNow) return false;
                if (session.StartDate <= DateTime.UtcNow) return false;

                var hasActiveBookings = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id);
                if (hasActiveBookings > 0) return false;

                return true;
            }
            private bool IsSessionAvailableForRemove(Session session)
            {
                if (session is null) return false;
                if (session.StartDate > DateTime.UtcNow) return false;
                if (session.StartDate <= DateTime.UtcNow && session.EndDate > DateTime.UtcNow) return false;

                var hasActiveBookings = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id);
                if (hasActiveBookings > 0) return false;

                return true;
            }
            #endregion
        }
    }

