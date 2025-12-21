using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; } //1-25
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Category-Session Relationship
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        #endregion

        #region Trainer-Session
        public Trainer Trainer { get; set; } = null!;
        public int TrainerId { get; set; }
        #endregion

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
    }
}
