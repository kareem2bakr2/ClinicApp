namespace ClinicApp.Repository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentByReceptionistAsync(int Receptid , DateTime start , DateTime end);

        IQueryable<Appointment> GetAppointmentById(int id);
        
    }
}
