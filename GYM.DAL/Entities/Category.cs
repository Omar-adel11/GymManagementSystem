using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;
        #region Category-Session Relationship
        public ICollection<Session> Sessions { get; set; } = null!;
        #endregion
    }
}
