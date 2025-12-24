using GYM.BLL.ModelViews.PlansModelViews;
using GYM.BLL.Services.Interfaces;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;


namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       

        public IEnumerable<PlanModelView> GetAllPlans()
        {
            var plans = _unitOfWork.Repository<Plan>().GetAll();

            if (plans is null || !plans.Any())
                return Enumerable.Empty<PlanModelView>();

            return plans.Select(p => new PlanModelView
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public PlanModelView? GetPlanById(int PlanId)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(PlanId);

            if (plan is null)
                return null;

            return new PlanModelView
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public PlanToUpdateModelView? GetPlanToUpdate(int PlanId)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(PlanId);

            if (plan is null || plan.IsActive == false)
                return null;

            return new PlanToUpdateModelView
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }

        public async Task<bool> UpdatePlan(int id, PlanToUpdateModelView input)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(id);

            if (plan is null || HasActiveMemberships(id))
                return false;

            plan.Description = input.Description;
            plan.DurationDays = input.DurationDays;
            plan.Price = input.Price;
            plan.Name = input.PlanName;
            plan.UpdateAt = DateTime.UtcNow;

            _unitOfWork.Repository<Plan>().Update(plan);
            return await _unitOfWork.SaveChangesAsync() > 0;

        }


        #region Helper Methods

        private bool HasActiveMemberships(int PlanId)
        {
            return _unitOfWork.Repository<Membership>()
                    .GetAll(m => m.PlanId == PlanId && m.Status == "Active")
                    .Any();
        }

        #endregion
    }
}