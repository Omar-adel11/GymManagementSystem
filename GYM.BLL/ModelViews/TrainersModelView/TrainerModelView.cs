using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.BLL.ModelViews.TrainersModelView
{
    public class TrainerModelView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public string? Street { get; set; } 
        public string? City { get; set; } 
        public string? BuildingNo { get; set; } 
    }
}
