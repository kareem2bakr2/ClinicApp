namespace ClinicApp.Repository
{
    public class ExerciseRepository :
        GenericRepository<Exercise>,
        IExerciseRepository
    {
        public ExerciseRepository(ClinicAppContext context) : base(context) { }

    }
}
