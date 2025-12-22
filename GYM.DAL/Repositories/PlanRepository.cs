using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Data.Contexts;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.DAL.Repositories
{
    public class PlanRepository(GYMDbContext _context) : IPlanRepository
    {
        public IEnumerable<Plan> GetAllPlans() =>_context.Plans.ToList();


        public Plan GetById(int id)
        {
            var plan = _context.Plans.Find(id);
            return plan!;
        }

        public void UpdatePlan(Plan plan)
        {
             _context.Plans.Update(plan);
        }
    }
}
