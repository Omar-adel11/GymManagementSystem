using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationDays { get; set; } //1-365
        public bool IsActive { get; set; }

        public ICollection<Membership> PlanMembers { get; set; } = null!;

    }
}
