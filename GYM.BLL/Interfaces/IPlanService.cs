using GYM.BLL.ModelViews;
using GYM.BLL.ModelViews.PlansModelViews;

namespace GYM.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<bool> UpdatePlan(int id, PlanToUpdateModelView input);
        PlanToUpdateModelView? GetPlanToUpdate(int PlanId);
        IEnumerable<PlanModelView> GetAllPlans();
        PlanModelView? GetPlanById(int PlanId);
        
    }
}