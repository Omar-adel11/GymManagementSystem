using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    public class Member : GYMUser
    {
        //CreateAt --> JoinDate
        public string? Photo { get; set; }

        #region Member-HealthRecord Relationship
        public HealthRecord HealthRecord { get; set; } = null!;

        #endregion

        public ICollection<Membership> Memberships { get; set; } = null!;
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;

    }
}
