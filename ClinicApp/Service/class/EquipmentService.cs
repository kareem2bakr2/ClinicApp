
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace ClinicApp.Service
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository equipmentRepo;
        private readonly IExerciseRepository exerciseRepo;
        public EquipmentService(
            IEquipmentRepository equipmentRepo,
            IExerciseRepository exerciseRepo
        ) { 
            this.equipmentRepo = equipmentRepo;
            this.exerciseRepo = exerciseRepo;
        }
        public async Task<EquipmentViewModel> CreateEquipmentStaticData() {

            return new EquipmentViewModel { 
                excersices = exerciseRepo
                .GetAll()
                .Select(e=>new EquipmentExerciseViewModel { 
                CategoryName = e.ExerciseCategory.Name,
                Name = e.Name,
                ID = e.ID ,
                Description = e.Description,
                Difficulty = e.Difficulty
                }).ToList()};

        }

        public async Task<Equipment> AddEquipmentAsync(EquipmentViewModel model)
        {
            
            var newEquipment = new Equipment() { 
                Exercises = exerciseRepo
                        .GetAll()
                        .Where(e=>model.SelectedExercises.Contains(e.ID))
                        .ToHashSet(),
                MerchantName = model.MerchantName,
                Name = model.Name,
                price = model.price,
            }; 
            await equipmentRepo.AddAsync(newEquipment);
            await equipmentRepo.SaveAsync();
            return newEquipment;
        }

        public async Task<ReturnResult> DeleteEquipmentAsync(int id)
        {
            await equipmentRepo.DeleteByIdAsync(id);
            await equipmentRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Equipment Deleted Successfully." };
        }

        public async Task<ReturnResult> EditEquipmentAsync(EquipmentViewModel model)
        {
            var equipment = await equipmentRepo.GetByIdAsync(model.Id);
            equipment.Exercises.Clear();
            equipment.Exercises =  exerciseRepo
                .GetAll()
                .Where(ex=> model.SelectedExercises.Contains(ex.ID))
                .ToHashSet();
            equipment.Name = model.Name;
            equipment.MerchantName = model.MerchantName;
            equipment.price = model.price;
            await equipmentRepo.SaveAsync();
            return new ReturnResult {
                flag = true, 
                message = "Eqipment Updated successfully" };
        }

        public async Task<ItemIndexViewModel> GetAllEquipmentsAsync(ItemIndexViewModel indexmodel)
        {
            var equipments =  equipmentRepo.GetAll()
                .Select(e => new ItemViewModel
                {
                    price = e.price,
                    Id = e.ID,
                    Name = e.Name,
                    MerchantName = e.MerchantName,
                });
            if(indexmodel.SearchTerm is not null)
            {
                equipments = equipments.Where(
                e => e.Id.ToString().Contains(indexmodel.SearchTerm) ||
                    e.price.ToString().Contains(indexmodel.SearchTerm) ||
                    e.MerchantName.Contains(indexmodel.SearchTerm) ||
                    e.Name.Contains(indexmodel.SearchTerm));
            }
            var count = equipments.Count();
            var equipmentsFiltered = await equipments
                .Skip( (indexmodel.pageNumber - 1) *  10 )
                .Take(10).ToListAsync();

            return new ItemIndexViewModel {
                Items = equipmentsFiltered,
                pageNumber = indexmodel.pageNumber,
                SearchTerm = indexmodel.SearchTerm,
                TotalPages =(int) Math.Ceiling(count  / (10.0)),

            };
        }

        public async Task<EquipmentViewModel> GetEquipmentDetailsByIdAsync(int id)
        {
            var equipment = await equipmentRepo.GetByIdAsync(id);
            if (equipment is null) return null;
            return new EquipmentViewModel
            {
                Id = equipment.ID,
                Name = equipment.Name,
                MerchantName = equipment.MerchantName,
                price = equipment.price,
                excersices = equipment.Exercises
                            .Select(e => new EquipmentExerciseViewModel
                            {
                                ID = e.ID,
                                CategoryName = e.ExerciseCategory?.Name,
                                Name = e.Name,
                                Description = e.Description,
                                Difficulty = e.Difficulty
                            })
                            .ToList()
                ,
                SelectedExercises = equipment.Exercises.Select(e => e.ID).ToList()
            };

        }
    }
}
