namespace ClinicApp.Repository
{
    public class TreatmentPlanExercisesRepository :
            GenericRepository<TreatmentPlanExercises>,
            ITreatmentPlanExercisesRepository
    {
        public TreatmentPlanExercisesRepository(ClinicAppContext clinicApp) : base(clinicApp)
        {
        }

        public ReturnResult Restore(int TreatmentID, int ExerciseID)
        {
            var entity = _context
                .TreatmentPlanExercises
                .Where(e => e.exerciseID == ExerciseID && e.treatmentPlanID == TreatmentID)
                .FirstOrDefault();
            if (entity is null) return new ReturnResult { flag = false, message = "Failed to Find Entery" };
            entity.isDeleted = false;
            return new ReturnResult { flag = true, message = "Restored Successfully" };
        }
    }
}
