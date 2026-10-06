using Microsoft.EntityFrameworkCore.Storage.Json;

namespace ClinicApp.Service
{
    public class ExerciseService : IExerciseService
    {

        private readonly IExerciseRepository exerciseRepo;
        private readonly IEquipmentRepository equipmentRepo;
        private readonly IMuscleRepository muscleRepo;
        private readonly IExerciseCategoryRepository categoryRepo;

        public ExerciseService(IExerciseRepository exerciseRepo ,
            IMuscleRepository muscleRepo ,
            IEquipmentRepository equipmentRepo,
            IExerciseCategoryRepository categoryRepo
            ) {
            this.categoryRepo = categoryRepo;
            this.exerciseRepo = exerciseRepo;
            this.equipmentRepo = equipmentRepo;
            this.muscleRepo = muscleRepo;
        }

        public async  Task<Exercise> AddExerciseEquipmentAsync(int ExID, List<int> EquipmentIDs)
        {   
            var Excersie = await exerciseRepo.GetByIdAsync(ExID);
            if (Excersie == null) return null;

            foreach (var equipID in EquipmentIDs) {
                
                var equip = await equipmentRepo.GetByIdAsync(equipID);
                if(equip is null) continue;
                if (Excersie.Equipments.Contains(equip)){
                
                    Excersie.Equipments.FirstOrDefault(e => e == equip).isDeleted = false;
                    continue;
             
                }
                Excersie.Equipments.Add(equip);
            }
            
            await exerciseRepo.SaveAsync();
            return Excersie;
        }   

        public async Task<Exercise> AddExerciseMuscleAsync(int ExID, List<int> MuscleIDs)
        {
            var exercise = await exerciseRepo.GetByIdAsync(ExID);
            if(exercise is null) return null;
            
            foreach(var  muscleID in MuscleIDs)
            {
                var muscle = muscleRepo
                    .GetAll()
                    .FirstOrDefault(e => e.ID == muscleID);
                
                if(muscle is null) continue;
                if(exercise.Muscles.Contains(muscle))
                {
                    exercise
                        .Muscles
                        .FirstOrDefault( e => e == muscle )
                        .isDeleted = false;
                    continue;
                }

                exercise.Muscles.Add(muscle);



            }
            await exerciseRepo.SaveAsync(); 
            return exercise;

        }

        public async Task<Exercise> CreateExerciseAsync(ExerciseCreateModelView model)
        {
            var ex = new Exercise { 
                Name = model.Name,
                Instructions = model.Instructions,
                Description = model.Description,
                Muscles = muscleRepo.GetAll().Where(e=>model.SelectedMuscles.Contains(e.ID)).ToHashSet(),
                CategoryID = model.CategoryID,
                Difficulty = model.Difficulty,
                NumberOfSets = model.NumberOfSets,
                VideoURL = model.VideoURL,
                Equipments = equipmentRepo.GetAll().Where(e=>model.SelectedEquipments.Contains(e.ID)).ToHashSet(),
                
            };  
            await exerciseRepo.AddAsync(ex);
            await exerciseRepo.SaveAsync();

            return ex;
        }

        public async Task<ReturnResult> DeleteExerciseAsync(int ExID)
        {
            var ex = await exerciseRepo.GetByIdAsync(ExID);
            if(ex is null )return new ReturnResult { flag = false ,message = "Can't find Exercise."};
            
            await exerciseRepo.DeleteByIdAsync(ExID);
            await exerciseRepo.SaveAsync();

            return new ReturnResult { flag = true  , message = "Deleted Successfully."};
        }

        public async Task<ExerciseCreateModelView> GetAllCreateStaticDataAsync()
        {
            return new ExerciseCreateModelView {
                allCategories = categoryRepo.GetAll().Select(c=>new SelectListModelView { ID =  c.ID , Name = c.Name}).ToList(),
                allEquipmetns = equipmentRepo.GetAll().Select(e=>new SelectListModelView { Name = e.Name , ID = e.ID}).ToList(),
                allMuscles = muscleRepo.GetAll().Select(m=>new SelectListModelView { ID = m.ID , Name = m.Name}).ToList(),
                
            };
        }

        public async Task<ExcerciseIndexViewModel> GetAllExercisesAsync(ExcerciseIndexViewModel filter)
        {
            var items = exerciseRepo.GetAll();
            if (filter.ssearchName is not null)
                items = items
                    .Where(e => e.Instructions.Contains(filter.ssearchName) ||
                    e.NumberOfSets.ToString().Contains(filter.ssearchName) ||
                    e.Description.Contains(filter.ssearchName) ||
                    e.ExerciseCategory.Name.Contains(filter.ssearchName)
                );
            items = items
                .Where(e => 
                e.NumberOfSets >= filter.NumberOfSetsMinFilter &&
                e.NumberOfSets <= filter.NumberOfSetsMaxFilter);
            if(filter.difficultyFilter != null)
                items = items.Where(e => e.Difficulty == filter.difficultyFilter);
            var indexView = new ExcerciseIndexViewModel
            {
                pageNumber = filter.pageNumber,
                difficultyFilter = filter.difficultyFilter,
                NumberOfSetsMaxFilter = filter.NumberOfSetsMaxFilter,
                NumberOfSetsMinFilter = filter.NumberOfSetsMinFilter,
                pageSize = filter.pageSize,
                ssearchName = filter.ssearchName,
                totalPageCount = (int) Math.Ceiling((double) items.Count() / filter.pageSize),

            };
            indexView.items = items.Select(e => new ExerciseListModelView 
                {
                    CategoryID = e.CategoryID,
                    CategoryName = e.ExerciseCategory.Name,
                    Name = e.Name,
                    Difficulty =e.Difficulty, 
                    ID =e.ID,
                    NumberOfSets = e.NumberOfSets
                })
                .Skip( (filter.pageNumber - 1 )* filter.pageSize)
                .Take(filter.pageSize)
                .ToList();
            return indexView;
        }

        public async Task<ExerciseDetailsModelView> GetExerciseDetailsAsync(int id)
        {
            var exercise =await exerciseRepo.GetByIdAsync(id);
            return new ExerciseDetailsModelView
            {
                allcategories = categoryRepo.GetAll().Select(c=>new SelectListModelView {ID = c.ID ,Name = c.Name  }).ToList(),
                allEquipmetns = equipmentRepo.GetAll().Select(e => new SelectListModelView {ID = e.ID , Name = e.Name }).ToList(),
                allMuscles = muscleRepo.GetAll().Select(m=> new SelectListModelView { Name = m.Name , ID = m. ID }).ToList(),     
                ID = exercise.ID,
                NumberOfSets = exercise.NumberOfSets,
                CategoryID = exercise.CategoryID,
                CategoryName = exercise.ExerciseCategory?.Name,
                Description = exercise.Description,
                Difficulty = exercise.Difficulty,
                Name = exercise.Name,
                Instructions = exercise.Instructions,
                VideoURL = exercise.VideoURL,
                SelectedEquipments = exercise
                .Equipments
                .Select(e => new SelectListModelView
                {
                    ID = e.ID,
                    Name = e.Name
                })
                .ToList(),
                SelectedMuscles = exercise
                .Muscles
                .Select(m => new SelectListModelView
                {
                    ID = m.ID,
                    Name = m.Name
                }).ToList(),
                SelectedTreatmentPlans = exercise
                .TreatmentPlanExercises
                .Select(tp=> new SelectListModelView {
                    ID = tp.treatmentPlanID ,
                    Name =tp.TreatmentPlan?.Name
                })
                .ToList()
            };
        }

        public async Task<Exercise> RemoveExerciseEquipmentAsync(int ExID, int EquipmentID)
        {
            var exercise =await exerciseRepo.GetByIdAsync(ExID);
            var equip = await equipmentRepo.GetByIdAsync(EquipmentID);
            
            if (exercise is null || equip is null) return null;
            
            exercise.Equipments.Remove(equip);
            await exerciseRepo.SaveAsync();

            return exercise;
        }

        public async Task<Exercise> RemoveExerciseMuscleAsync(int ExID, int MuscleID)
        {
            var exercise = await exerciseRepo.GetByIdAsync(ExID);
            var Muscle = await muscleRepo.GetByIdAsync(MuscleID);

            if (exercise is null || Muscle is null) return null;

            exercise.Muscles.Remove(Muscle);
            await exerciseRepo.SaveAsync();

            return exercise;

        }

        public async Task<Exercise> UpdateExerciseAsync(ExerciseCreateModelView model)
        {
            var exercise = await exerciseRepo.GetByIdAsync(model.ID);

            if(exercise is null ) return null;

            exercise.Name = model.Name;
            exercise.VideoURL = model.VideoURL;
            exercise.Description = model.Description;
            exercise.CategoryID = model.CategoryID;
            exercise.Difficulty = model.Difficulty;
            exercise.Instructions = model.Instructions;
            exercise.NumberOfSets = model.NumberOfSets;
            exercise.Equipments.Clear();
            exercise.Equipments = equipmentRepo
                .GetAll()
                .Where(eq=>model.SelectedEquipments.Contains(eq.ID))
                .ToHashSet();
            exercise.Muscles.Clear();
            exercise.Muscles = muscleRepo.GetAll().Where(m => model.SelectedMuscles.Contains(m.ID)).ToHashSet();

            await exerciseRepo.SaveAsync();
            return exercise;

        }


    }
}
