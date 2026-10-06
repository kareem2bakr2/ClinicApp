namespace ClinicApp.Repository
{
    public interface IGenericRepository<T> where T: class,ILogsAttribuite 
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task Delete(T entity);
        Task AddAsync(T entity);
        Task SaveAsync();
    }
}
