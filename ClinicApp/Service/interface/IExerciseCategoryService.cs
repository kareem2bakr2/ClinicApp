namespace ClinicApp.Service
{
    public interface IExerciseCategoryService
    {
        
        Task<ExerciseCategoryIndexModelView> GetAllCategoriresAsync(ExerciseCategoryIndexModelView filter);
        Task<ExerciseCategory> EditExerciseCategoryAsync(ExerciseCategoryModelView model);
        Task<ExerciseCategory> CreateExerciseCategoryAsync(ExerciseCategoryModelView model);
        Task<ExerciseCategoryDetailsModelView> GetExerciseCategoryDetailsAsync(int id);
        Task<ReturnResult> DeleteExerciseAsync(int id);


    }
}
