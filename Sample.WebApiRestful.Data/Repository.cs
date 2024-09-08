using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sample.WebApiRestful.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        SampleWebApiContext _sampleWebApiContext;

        public Repository(SampleWebApiContext sampleWebApiContext)
        {
            _sampleWebApiContext = sampleWebApiContext;
        }

        public async Task<IEnumerable<T>> GetData(Expression<Func<T, bool>> expression)
        {
            //get all if not have condition
            if (expression == null)
            {
                return await _sampleWebApiContext.Set<T>().ToListAsync();
            }

            return await _sampleWebApiContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await _sampleWebApiContext.Set<T>().FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await _sampleWebApiContext.Set<T>().AddAsync(entity);
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            await _sampleWebApiContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            EntityEntry entityEntry = _sampleWebApiContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(T entity)
        {
            EntityEntry entityEntry = _sampleWebApiContext.Entry<T>(entity);
            entityEntry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            var entities = _sampleWebApiContext.Set<T>().Where(expression).ToList();

            if (entities.Count > 0)
            {
                _sampleWebApiContext.Set<T>().RemoveRange(entities);
            }
        }

        public virtual IQueryable<T> Table => _sampleWebApiContext.Set<T>();

        public async Task Commit()
        {
            await _sampleWebApiContext.SaveChangesAsync();
        }
    }
}
