using System.Security.Cryptography;

namespace ClinicApp.Repository
{
    public class GenericRepository<T> :
        IGenericRepository<T> where T : class,ILogsAttribuite
    {
        protected ClinicAppContext _context;
        protected DbSet<T> _dbSet;
        public GenericRepository(ClinicAppContext clinicApp) {
            _context = clinicApp;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            entity.isDeleted = true;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var enttity = await GetByIdAsync(id);
            enttity.isDeleted = true;
        }

        public  IQueryable<T> GetAll()
            
        {
            return  _dbSet.Where(x => x.isDeleted == false).AsQueryable();
            //return await _dbSet.Where(e=>e.isDeleted == false).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity is null || entity.isDeleted) return null;
            return entity ;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }
    }
}
