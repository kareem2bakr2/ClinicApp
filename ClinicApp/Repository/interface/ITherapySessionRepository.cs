namespace ClinicApp.Repository
{
    public interface ITherapySessionRepository : IGenericRepository<TherapySession>
    {
        Task<List<TherapySession>> GetSessionsByTherapistID( int therapistId);
    }
}
