namespace ClinicApp.Repository
{
    public interface ITreatmentPlanExercisesRepository : IGenericRepository<TreatmentPlanExercises>
    {
        ReturnResult Restore(int TreatmentID , int ExerciseID);
           
    }
}
