namespace ClinicApp.Repository
{
    public interface ITherapistRepository : IGenericRepository<Therapist>
    {
        Task<IEnumerable<Therapist>> GetMyStaffTherapistsAsync(int managerid);

        Task<IEnumerable<Therapist>> GetTherapistWithNoMGR();
        Task<Therapist> GetTherapistFromAppIdAsync(string AppId);

        Task<Therapist> GetTherapistWithAppointmentsSessionsPaymentAsync(int Therapistid);
    }
}
