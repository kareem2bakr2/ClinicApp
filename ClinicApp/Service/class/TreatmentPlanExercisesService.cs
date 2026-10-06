namespace ClinicApp.Service
{
    public class TreatmentPlanExercisesService : ITreatmentPlanExercisesService
    {
        private readonly ITreatmentPlanExercisesRepository treatmentPlanExercisesRepo;

        public TreatmentPlanExercisesService(
            ITreatmentPlanExercisesRepository treatmentPlanExercisesRepo)
        {
            this.treatmentPlanExercisesRepo = 
                    treatmentPlanExercisesRepo;
        }

    }
}
