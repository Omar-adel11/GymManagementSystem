using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.BLL.ModelViews.SessionsModelViews
{
    public class SessionModelView
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDAte { get; set; }
        public string TrainerName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int AvailableSlots { get; set; }

        // Computed properties
        public string DateDisplay => $"{StartDate:MMM dd , yyyy}";
        public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDAte:hh:mm tt}";
        public TimeSpan Duration => EndDAte - StartDate;

        public string Status
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return "Upcoming";
                else if (StartDate <= DateTime.Now && EndDAte >= DateTime.Now)
                    return "Ongoing";
                else
                    return "Completed";
            }
        }
    }
}
