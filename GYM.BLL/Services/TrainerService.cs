using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.TrainersModelView;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class TrainerService(IUnitOfWork _unitOfWork,IMapper _mapper) : ITrainerService
    {
        public ICollection<TrainerModelView> GetAllTrainers()
        {
            var trainers = _unitOfWork.Repository<Trainer>().GetAll();
            if (trainers == null || !trainers.Any())
            {
                return new List<TrainerModelView>();
            }
            return _mapper.Map<ICollection<TrainerModelView>>(trainers);
        }

        public TrainerModelView? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.Repository<Trainer>().GetById(id);
            if (trainer == null || checkEmailExistence(trainer.Email) || checkPhoneExistence(trainer.phone) )
            {
                return null;
            }
            return _mapper.Map<TrainerModelView>(trainer);
        }
 
        public async Task<bool> CreateTrainer(TrainerToBeCreatedModelView trainerToBeCreatedModelView)
        {
            if( checkEmailExistence(trainerToBeCreatedModelView.Email) || checkPhoneExistence(trainerToBeCreatedModelView.Phone))
                return false;

            try
            {
                var trainer = _mapper.Map<Trainer>(trainerToBeCreatedModelView);
                _unitOfWork.Repository<Trainer>().Add(trainer);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }


        }

        public TrainerToBeUpdatedModelView? GetTrainerToBeUpdated(int id)
        {
            var trainer = _unitOfWork.Repository<Trainer>().GetById(id);
            if (trainer == null)
            {
                return null;
            }
            try
            {
                return _mapper.Map<TrainerToBeUpdatedModelView>(trainer);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateTrainer(int id, TrainerToBeUpdatedModelView trainerToBeUpdatedModelView)
        {
            var trainer = _unitOfWork.Repository<Trainer>().GetById(id);
            if (trainer == null  || checkEmailExistence(trainer.Email) || checkPhoneExistence(trainer.phone))
            {
                return false;
            }
            else
            {
                try
                {
                    _mapper.Map(trainerToBeUpdatedModelView, trainer);

                    _unitOfWork.Repository<Trainer>().Update(trainer);
                    return await _unitOfWork.SaveChangesAsync() > 0;
                } catch
                {
                    return false;
                }
            }
        }
     
        public async Task<bool> DeleteTrainer(int id)
        {
            var trainer = _unitOfWork.Repository<Trainer>().GetById(id);
            if (trainer == null || HasActiveSession(id))
            {
                return false;

            }
            try
            {
                _unitOfWork.Repository<Trainer>().Delete(id);
                 return await _unitOfWork.SaveChangesAsync() > 0;
      
            }
            catch
            {
                return false;
            }
        }

        private bool HasActiveSession(int id)
        {
            var activeSession = _unitOfWork.Repository<Session>().GetAll(s=>s.TrainerId == id
                                                                    && s.StartDate > DateTime.Now);
            return activeSession.Any();
        }
        private bool checkEmailExistence(string Email)
        {
            var isExist = _unitOfWork.Repository<Trainer>().GetAll(m => m.Email == Email).Any();
            return isExist;
        }

        private bool checkPhoneExistence(string phone)
        {
            var isExist = _unitOfWork.Repository<Trainer>().GetAll(m => m.phone == phone).Any();
            return isExist;
        }

    }
}
