using Microsoft.EntityFrameworkCore;
using SoberProgress.Domain;
using SoberProgress.Domain.ModelInterfaces;

namespace SoberProgress.DataAccess
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        public EntityRepository()
        {
            using var context = new AlcoDbContext();
            context.Database.EnsureCreated();
        }

        public IEnumerable<T> ReadAll()
        {
            using var context = new AlcoDbContext();
            return context.Set<T>().ToList();
        }

        public T ReadById(int id)
        {
            using var context = new AlcoDbContext();
            return context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public void Create(T item)
        {
            using var context = new AlcoDbContext();
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            using var context = new AlcoDbContext();
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            using var context = new AlcoDbContext();
            var item = context.Set<T>().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }
    }
}