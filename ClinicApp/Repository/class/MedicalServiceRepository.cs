namespace ClinicApp.Repository
{
    public class MedicalServiceRepository : 
        GenericRepository<MedicalService>,
        IMedicalServiceRepository
    {
        public MedicalServiceRepository(ClinicAppContext context) : base(context) { }
    }
}
