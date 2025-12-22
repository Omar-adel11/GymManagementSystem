using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.BLL.ModelViews.MemebersModelViews
{
    public class MemberToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        public string Photo { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email can't exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(11, ErrorMessage = "PhoneNumber can't exceed 11 characters")]
        [RegularExpression(@"(010|011|012|015)\d{8}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Building number type is required")]
        [Range(1, 9000, ErrorMessage = "Building number must be between 1 and 9000")]
        public int BuildingNo { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [MaxLength(30, ErrorMessage = "Name can't exceed 30 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [MaxLength(30, ErrorMessage = "Name can't exceed 30 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string City { get; set; } = null!;
    }
}
