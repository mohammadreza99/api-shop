using Microsoft.EntityFrameworkCore;
using ShopApi.DataLayer.Data;
using System.Linq;

namespace ShopApi.DataLayer.Repositories
{
    public class GenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public virtual T GetById(object id)
        {
            return _context.Set<T>().Find(id);
            //, params string[] includeProperties
            //var propertyName = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)DatabaseContext).ObjectContext
            //.CreateObjectSet<T>().EntitySet.ElementType.KeyMembers.Single().Name;

            //var parameter = Expression.Parameter(typeof(T), "e");
            //var predicate = Expression.Lambda<Func<T, bool>>(
            //    Expression.Equal(
            //        Expression.PropertyOrField(parameter, propertyName),
            //        Expression.Constant(id)),
            //    parameter);

            //var query = DbSet<T>.AsQueryable();
            //if (includeProperties != null && includeProperties.Length > 0)
            //    query = includeProperties.Aggregate(query, System.Data.Entity.QueryableExtensions.Include);
            //return query.FirstOrDefault(predicate);
        }

        public virtual T Add(T entity)
        {
            _context.Add(entity);
            return entity;
        }

        public virtual T Update(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _context.Attach(entity);
            _context.Remove(entity);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }
    }
}
