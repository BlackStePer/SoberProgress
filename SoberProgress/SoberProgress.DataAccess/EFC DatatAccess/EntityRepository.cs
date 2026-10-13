using Microsoft.EntityFrameworkCore;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.DataAccess
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly AlcoDbContext _context;
        private bool _disposed = false;

        public EntityRepository()
        {
            _context = new AlcoDbContext();
            _context.Database.EnsureCreated();
        }

        public IEnumerable<T> ReadAll()
        {
            return _context.Set<T>().ToList();
        }

        public T ReadById(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public void Create(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Set<T>().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _context.Set<T>().Remove(item);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        ~EntityRepository()
        {
            Dispose(false);
        }
    }
}