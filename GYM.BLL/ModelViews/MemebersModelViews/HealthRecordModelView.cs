using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.BLL.ModelViews.MemebersModelViews
{
    public class HealthRecordModelView
    {
        [Required(ErrorMessage = "Weight is required")]
        [Range(20, 500, ErrorMessage = "Weight must be between 20 and 500")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Height is required")]
        [Range(50, 300, ErrorMessage = "Height must be between 50 and 300")]
        public double Height { get; set; }

        [Required(ErrorMessage = "Blood Type is required")]
        [MaxLength(3, ErrorMessage = "Blood Type can't exceed 3 characters")]
        public string BloodType { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
    }
}
