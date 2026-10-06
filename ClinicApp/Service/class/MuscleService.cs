using System.Security.Cryptography.Pkcs;

namespace ClinicApp.Service
{
    public class MuscleService  : IMuscleService
    {
        private readonly IMuscleRepository muscleRepo;

        public MuscleService(IMuscleRepository muscleRepo) {

            this.muscleRepo = muscleRepo;

        }
        public async Task<MuscleIndexViewModel> GetAllAsync(MuscleIndexViewModel filter) 
        {
            var all_muscles = muscleRepo
                .GetAll();
            
            filter.totalPages =(int) Math.Ceiling((decimal) all_muscles.Count() /filter.pageSize);

            filter._items = all_muscles.Select(e => new MuscleViewModel {id = e.ID , name=e.Name }).ToList();
            
            return filter;
        }

        public async Task<MuscleViewModel> GetDetails(int id) { 
            var muscle = await muscleRepo.GetByIdAsync(id);
            if (muscle == null) return null;

            return new MuscleViewModel {
                id = id,
                name = muscle.Name ,
                _exercises = muscle.Exercises.Select(e=>new ExerciseLinks { Id= e.ID , Displayname = e.Name}).ToList()
            };
        
        }

        public async Task<ReturnResult> EditMuscle(MuscleViewModel model) {
            
            var muscle = await muscleRepo.GetByIdAsync(model.id);

            if(muscle is null) 
                new ReturnResult { flag = false , message = "Failed To Edit , Please Try Again Later."};
            muscle.Name = model.name;
            await muscleRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Updaated Succeszfully" };
        }
        public async Task<ReturnResult> AddMuscle(MuscleViewModel model)
        {
            if(model.name is null) 
                return new ReturnResult { flag = false, message = "Failed to Add ,Please try again later" };

            var muscle = new Muscle { Name = model.name };
            await muscleRepo.AddAsync(muscle);
            await muscleRepo.SaveAsync();
            return new ReturnResult { flag = true, message = "Created successfully" };
        }

    }
}
