namespace ClinicApp.Repository 
{ 
    public class ExerciseCategoryRepository :
                    GenericRepository<ExerciseCategory>,
                        IExerciseCategoryRepository
    {
        public ExerciseCategoryRepository(ClinicAppContext context ) : base(context) { }
    }
}
