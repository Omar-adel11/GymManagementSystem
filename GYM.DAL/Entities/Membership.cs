using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    public class Membership : BaseEntity
    {
        //StartDate --> CreateAt
        public DateTime EndDate { get; set; }
        public string Status {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            }
            
        }
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = null!;
        public int PlanId { get; set; }
    }
}
