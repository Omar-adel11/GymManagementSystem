using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.BLL.Interfaces
{
    public interface IMemberService
    {
        ICollection<Member> GetAllMembers();
    }
}
