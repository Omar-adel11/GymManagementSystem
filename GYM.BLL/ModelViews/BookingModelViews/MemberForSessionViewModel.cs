using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.BLL.ModelViews.BookingModelViews
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public int SessionId { get; set; }
        public string BookingDate { get; set; } = null!;
        public bool IsAttended { get; set; }
    }
}
