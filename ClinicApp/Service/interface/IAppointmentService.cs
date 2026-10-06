
namespace ClinicApp.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentIndexViewModel> GetAppointmentsAsync(FilterAppointmentListViewModel filter);
        
        Task<AppointmentDetailsViewModel> GetAppointmentDetailsAsync(int id );
        Task<ReturnResult> CancelAppointmenAsync(int id);

        Task<ReturnResult> PayAppointmentByPatientAsync(AppointmentPayViewModel payInfo);
        Task<ReturnResult> EditAppointmentAsync(AppointmentEditViewModel appointment);
        
        Task<ReturnResult> ScheduleAppointmentAsync(CompleteAppointmentViewModel model);

        Task<Appointment> AddAppointmentAsync(AppointmentCreateViewModel appointment);
        
        Task<AppointmentCreateViewModel> GetCreateStaticDataAsync(int patientid);
    }
}
