using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Service
{
    public class ExerciseCategoryService : IExerciseCategoryService
    {
        private readonly IExerciseCategoryRepository exerciseCategoryRepo;
        public ExerciseCategoryService(
            IExerciseCategoryRepository exerciseCategoryRepo) 
        {
            this.exerciseCategoryRepo = exerciseCategoryRepo;
        }

        public async Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategoryModelView model)
        {
            if (model == null) return null;

            var ex = new ExerciseCategory { Name = model.Name };
            await exerciseCategoryRepo.AddAsync(ex);
            return ex;
        }

        public async Task<ReturnResult> DeleteExerciseAsync(int id)
        {
            var ex = await exerciseCategoryRepo.GetByIdAsync(id);
            if (ex is null) return new ReturnResult { flag = false, message = "Failed to Delete Entity" };
            
            await exerciseCategoryRepo.Delete(ex);
            await exerciseCategoryRepo.SaveAsync();
             return new ReturnResult { flag = true, message = "Deleted Ssuccessfully" };
        }

        public async Task<ExerciseCategory> EditExerciseCategoryAsync(ExerciseCategoryModelView model)
        {
            var modelDB = await exerciseCategoryRepo.GetByIdAsync(model.ID);
            if (modelDB is null) return null;

            modelDB.Name = model.Name;
            await exerciseCategoryRepo.SaveAsync();

            return modelDB;
        }

        public async Task<ExerciseCategoryIndexModelView> GetAllCategoriresAsync(ExerciseCategoryIndexModelView filter)
        {

            var items = exerciseCategoryRepo.GetAll();

            if(filter.searchName is not null) { 
                items = items.Where(c=>c.Name.Contains(filter.searchName));
            }

            filter.PagesCount = items.Count();
            filter._items = items.Skip((filter.PageNumber - 1) * filter.PageSize)
                                    .Take(filter.PageSize)
                                    .Select(c=> new ExerciseCategoryModelView 
                                    {   ID = c.ID ,
                                        Name = c.Name}
                                    )
                                    .ToList();
            return filter;
        }

        public async Task<ExerciseCategoryDetailsModelView> GetExerciseCategoryDetailsAsync(int id)
        {
            var model = await exerciseCategoryRepo.GetByIdAsync(id);
            if (model is null) return null;

            return new ExerciseCategoryDetailsModelView
            {
                ID = model.ID,
                Name = model.Name,
                _items = model.Exercises.
                Select(e => new ExerciseLinks 
                {
                    Id = e.ID,
                    Displayname = e.Name
                }).ToList()
            };   
        }


    }
}
