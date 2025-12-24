using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities.Enum;

namespace GYM.BLL.ModelViews.TrainersModelView
{
    public class TrainerToBeUpdatedModelView
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Specialities Specialization { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int BuildingNo { get; set; }
    }
}
