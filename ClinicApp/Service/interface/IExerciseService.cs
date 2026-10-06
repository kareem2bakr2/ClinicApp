namespace ClinicApp.Service
{
    public interface IExerciseService
    {
        Task<ExerciseDetailsModelView> GetExerciseDetailsAsync(int id);
        Task<ExcerciseIndexViewModel> GetAllExercisesAsync(ExcerciseIndexViewModel filter);
        Task<ExerciseCreateModelView> GetAllCreateStaticDataAsync();
        Task<Exercise> CreateExerciseAsync(ExerciseCreateModelView model);
        Task<Exercise> UpdateExerciseAsync(ExerciseCreateModelView model);
        Task<Exercise> AddExerciseMuscleAsync(int ExID, List<int> MuscleIDs);
        Task<Exercise> AddExerciseEquipmentAsync(int ExID, List<int> EquipmentIDs);
        Task<Exercise> RemoveExerciseMuscleAsync(int ExID  ,int MuscleID);
        Task<Exercise> RemoveExerciseEquipmentAsync(int ExID  ,int EquipmentID);
        Task<ReturnResult> DeleteExerciseAsync(int ExID);
    }
}
