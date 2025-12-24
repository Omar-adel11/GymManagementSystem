using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.SessionsModelViews;

namespace GYM.BLL.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionModelView> GetAllSessions();
        SessionModelView? GetSessionById(int sessionId);
        Task<bool> CreateSession(CreateSessionModelView input);
        UpdateSession? GetSessionToUpdate(int sessionId);
        Task<bool> UpdateSession(int sessionId, UpdateSession input);
        Task<bool>  RemoveSession(int sessionId);
       
    }
}
