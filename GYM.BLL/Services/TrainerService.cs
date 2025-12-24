using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.BLL.Interfaces;
using GYM.BLL.ModelViews.TrainersModelView;
using GYM.DAL.Entities;
using GYM.DAL.Interfaces;

namespace GYM.BLL.Services
{
    public class TrainerService(IUnitOfWork _unitOfWork) : ITrainerService
    {
        public ICollection<TrainerModelView> GetAllTrainers()
        {
            var trainers = _unitOfWork.Repository<Trainer>().GetAll();
            if (trainers == null || !trainers.Any())
            {
                return new List<TrainerModelView>();
            }
            var trainerModelViews = trainers.Select(t => new TrainerModelView
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.phone,
                Specialization = t.Specialities.ToString(),
               
            }).ToList();

            return trainerModelViews;
        }

        public TrainerModelView? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.Repository<Trainer>().GetById(id);
            if (trainer == null || checkEmailExistence(trainer.Email) || checkPhoneExistence(trainer.phone) )
            {
                return null;
            }
            var trainerModelView = new TrainerModelView
            {
               
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.phone,
                Specialization = trainer.Specialities.ToString(),
                DateOfBirth = trainer.DateOfBirth,
                Street = trainer.Address?.Street,
                City = trainer.Address?.City,
                BuildingNo = trainer.Address?.BuildingNo.ToString()

            };

            return trainerModelView;
        }
 
        public async Task<bool> CreateTrainer(TrainerToBeCreatedModelView trainerToBeCreatedModelView)
        {
            if( checkEmailExistence(trainerToBeCreatedModelView.Email) || checkPhoneExistence(trainerToBeCreatedModelView.Phone))
                return false;

            try
            {
                var trainer = new Trainer
                {
                    Name = trainerToBeCreatedModelView.Name,
                    Email = trainerToBeCreatedModelView.Email,
                    phone = trainerToBeCreatedModelView.Phone,
                    Specialities = trainerToBeCreatedModelView.Specialization,
                    DateOfBirth = trainerToBeCreatedModelView.DateOfBirth,

                    Address = new Address
                    {
                        Street = trainerToBeCreatedModelView.Street,
                        City = trainerToBeCreatedModelView.City,
                        BuildingNo = trainerToBeCreatedModelView.BuildingNo
                    }

                };
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
                var trainerToBeUpdatedModelView = new TrainerToBeUpdatedModelView
                {
                    Email = trainer.Email,
                    Phone = trainer.phone,
                    Specialization = trainer.Specialities,
                    Street = trainer.Address?.Street,
                    City = trainer.Address?.City,
                    BuildingNo = trainer.Address?.BuildingNo ?? 0
                };

                return trainerToBeUpdatedModelView;
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
                    var updatedTrainer = new Trainer
                    {

                        Email = trainerToBeUpdatedModelView.Email,
                        phone = trainerToBeUpdatedModelView.Phone,
                        Specialities = trainerToBeUpdatedModelView.Specialization,

                        Address = new Address
                        {
                            Street = trainerToBeUpdatedModelView.Street,
                            City = trainerToBeUpdatedModelView.City,
                            BuildingNo = trainerToBeUpdatedModelView.BuildingNo
                        },
                        UpdateAt = DateTime.Now

                    };

                    _unitOfWork.Repository<Trainer>().Update(updatedTrainer);
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
