namespace ClinicApp.Service
{
    public interface ITherapySessionService
    {
        Task<TherapySessionIndexModelView> GetAllIndexAsync(TherapySessionIndexModelView model);

        Task<ReturnResult> CancelSessionAsync(int id);
        Task<TherapySessionModelView> GetDetialsAsync(int id);
        Task<ReturnResult> CompleteSessionAsync(SessionCompleteViewModel model);
        Task<ReturnResult> MarkAsNoShowAsync(int id);
        Task<ReturnResult> ReScheduleSessionAsync(SessionResheduleViewMdel model);
    }
}

