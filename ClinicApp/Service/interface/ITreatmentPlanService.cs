namespace ClinicApp.Service
{
    public interface ITreatmentPlanService
    {
        Task<TreatmentPlanIndexModelView> GetAllTreatmentPlansAsync(TreatmentPlanIndexModelView filter);
        Task<TreatmentPlan> CreateTreatmentPlanAsync(TreatmentPlanCreateModelview model);
        Task<TreatmentPlan> EditTreatmentPlanAsync(TreatmentPlanCreateModelview model);
        Task<ReturnResult> DeleteTreatmentPlanAsync(int id);
        Task<TreatmentPlan> AddNewExerciseAsync(int TreatPlanid, List<int> ExerciseId);
        Task<TreatmentPlanModelview> GetDetailsAsync(int id);

        Task<ReturnResult> RemoveExerciseAsync(int TreatPlanid, int ExerciseId);

    }
}
