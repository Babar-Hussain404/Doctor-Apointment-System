using DocApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DocApp.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DocAppContext _dbContext;
        private DbSet<T> _dbSet;
        
        public GenericRepository(DocAppContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public void Add(T obj)
        {
            _dbSet.Add(obj);
        }
        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = _dbSet.Find(id);
            _dbSet.Remove(existing);
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
