namespace ClinicApp.Repository
{
    public class MuscleRepository:
        GenericRepository<Muscle>
        ,IMuscleRepository
    {
        public MuscleRepository(ClinicAppContext context):base(context) { }
    }
}
