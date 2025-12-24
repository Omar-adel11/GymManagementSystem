using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.ModelViews.TrainersModelView;

namespace GYM.BLL.Interfaces
{
    public interface ITrainerService
    {
        ICollection<TrainerModelView> GetAllTrainers();
         TrainerModelView? GetTrainerDetails(int id);
        Task<bool> CreateTrainer(TrainerToBeCreatedModelView trainerToBeCreatedModelView);
        TrainerToBeUpdatedModelView? GetTrainerToBeUpdated(int id);
        Task<bool> UpdateTrainer(int id, TrainerToBeUpdatedModelView trainerToBeUpdatedModelView);
        Task<bool> DeleteTrainer(int id);

    }
}
