using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;

namespace GYM.DAL.Interfaces
{
    public interface IPlanRepository
    {
        Plan GetById(int id);
        IEnumerable<Plan> GetAllPlans();
        void UpdatePlan(Plan plan);
    }
}
