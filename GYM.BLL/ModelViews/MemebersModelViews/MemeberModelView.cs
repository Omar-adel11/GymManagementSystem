using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.BLL.ModelViews.MemebersModelViews
{
    public class MemeberModelView
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email can't exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11, ErrorMessage = "PhoneNumber can't exceed 11 characters")]
        [RegularExpression(@"(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(6, ErrorMessage= "Gender can't exceed 6 characters")]
        public string Gender { get; set; } = null!;

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Health record is required")]
        public HealthRecordModelView? HealthRecord { get; set; }

        public string? PlanName { get; set; }
        public string? Address { get; set; }
        public string? BirthDate { get; set; }
        public string? MembershipStartDate { get; set; }
        public string? MembershipEndDate { get; set; }

    }
}
