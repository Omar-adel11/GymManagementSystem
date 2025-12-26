using System.Numerics;
using AutoMapper;
using GYM.BLL.ModelViews.PlansModelViews;
using GYM.BLL.ModelViews.TrainersModelView;
using GYM.BLL.Services.Interfaces;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;


namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       

        public IEnumerable<PlanModelView> GetAllPlans()
        {
            var plans = _unitOfWork.Repository<Plan>().GetAll();

            if (plans is null || !plans.Any())
                return Enumerable.Empty<PlanModelView>();

            return _mapper.Map<IEnumerable<PlanModelView>>(plans);
        }

        public PlanModelView? GetPlanById(int PlanId)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(PlanId);

            if (plan is null)
                return null;

            return _mapper.Map<PlanModelView>(plan);
        }

        public PlanToUpdateModelView? GetPlanToUpdate(int PlanId)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(PlanId);

            if (plan is null || plan.IsActive == false)
                return null;

            return _mapper.Map< PlanToUpdateModelView>(plan);
        }

        public async Task<bool> UpdatePlan(int id, PlanToUpdateModelView input)
        {
            var plan = _unitOfWork.Repository<Plan>().GetById(id);

            if (plan is null || HasActiveMemberships(id))
                return false;

            _mapper.Map(input, plan);

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