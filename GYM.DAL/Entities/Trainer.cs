using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities.Enum;

namespace GYM.DAL.Entities
{
    public class Trainer : GYMUser
    {
        //CreateAt --> HireDate
        public Specialities Specialities { get; set; }
        #region Trainer-Session

        public ICollection<Session> Sessions { get; set; } = null!;
        #endregion
    }
}
