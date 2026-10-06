namespace ClinicApp.Repository
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient> PatientWithSessions(int id);

    }
}
