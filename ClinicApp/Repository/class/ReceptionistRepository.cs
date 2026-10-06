namespace ClinicApp.Repository
{
    public class ReceptionistRepository :
        GenericRepository<Receptionist>
        , IReceptionistRepository
    {
        public ReceptionistRepository(ClinicAppContext clinicApp) : base(clinicApp)
        {
        }
    }
}
