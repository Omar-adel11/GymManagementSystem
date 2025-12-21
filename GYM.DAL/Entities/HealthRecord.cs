using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.DAL.Entities
{
    //1-1 with member
    public class HealthRecord : BaseEntity
    {
        public double Weight { get; set; }
        public double Height { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }

       
    }
}
