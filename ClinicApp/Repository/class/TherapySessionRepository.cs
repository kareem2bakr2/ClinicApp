namespace ClinicApp.Repository
{
    public class TherapySessionRepository:
        GenericRepository<TherapySession>,
        ITherapySessionRepository
    {
        public TherapySessionRepository(ClinicAppContext clinicAppContext):base(clinicAppContext)
        {
            
        }

        public async Task<List<TherapySession>> GetSessionsByTherapistID(int therapistId)
        {
            return await _context.TherapySessions
                        .Where(s => s.TheeapistID == therapistId)
                        .ToListAsync();
        }
    }

}
