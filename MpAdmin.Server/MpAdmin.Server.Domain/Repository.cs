using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using WorkReportApp.DAL;

namespace WorkReportApp.Domain
{
    public class Repository<T, TKey> : IRepository<T, TKey> where T : class, IEntity<TKey>
    {

        protected MpAdminContext dbContext;

        public Repository(MpAdminContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public T Save(T entity)
        {
            if (entity is IEntity<short> shortkeyObj)
            {
                return shortkeyObj.Id == 0 ? dbContext.Add(entity).Entity : dbContext.Update(entity).Entity;
            }

            if (entity is IEntity<int> intkeyObj)
            {
                return intkeyObj.Id == 0 ? dbContext.Add(entity).Entity : dbContext.Update(entity).Entity;
            }
            else if (entity is IEntity<long> longkeyObj)
            {
                return longkeyObj.Id == 0 ? dbContext.Add(entity).Entity : dbContext.Update(entity).Entity;
            }
            else if (entity is IEntity<Guid> guidkeyObj)
            {
                if (guidkeyObj.Id == Guid.Empty)
                {
                    guidkeyObj.Id = Guid.NewGuid();
                    return dbContext.Add(entity).Entity;
                }
                else
                    return dbContext.Update(entity).Entity;
            }
            else
                throw new Exception();
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (entity is IEntity<short> shortkeyObj)
            {
                return shortkeyObj.Id == 0
                    ? (await dbContext.AddAsync(entity)).Entity
                    : dbContext.Update(entity).Entity;
            }

            if (entity is IEntity<int> intkeyObj)
            {
                return intkeyObj.Id == 0 ? (await dbContext.AddAsync(entity)).Entity : dbContext.Update(entity).Entity;
            }

            if (entity is IEntity<long> longkeyObj)
            {
                return longkeyObj.Id == 0 ? (await dbContext.AddAsync(entity)).Entity : dbContext.Update(entity).Entity;
            }
            else if (entity is IEntity<Guid> guidkeyObj)
            {
                if (guidkeyObj.Id == Guid.Empty)
                {
                    guidkeyObj.Id = Guid.NewGuid();
                    return (await dbContext.AddAsync(entity)).Entity;
                }
                else
                    return dbContext.Update(entity).Entity;
            }
            else
                throw new Exception();
        }

        public T Create(T entity)
        {
            return dbContext.Add(entity).Entity;
        }

        public void Update(T entity)
        {
            dbContext.Update(entity);
        }

        public void Delete(T entity)
        {
            dbContext.Remove(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => dbContext.Remove(entity));
        }


        public void DeleteAll(Expression<Func<T, bool>> predicate)
        {
            dbContext.RemoveRange(dbContext.Set<T>().Where(predicate));
        }

        public IQueryable<T> Get()
        {
            return dbContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate);
        }

        public async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
                return await Task.Run(() => dbContext.Set<T>().Where(predicate));
            return await Task.Run(() => dbContext.Set<T>());
        }

        public T Single()
        {
            return dbContext.Set<T>().Single();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate).Single();
        }

        public T SingleOrDefault()
        {
            return dbContext.Set<T>().SingleOrDefault();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate).SingleOrDefault();
        }

        public T First()
        {
            return dbContext.Set<T>().First();
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate).First();
        }

        public T FirstOrDefault()
        {
            return dbContext.Set<T>().FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate).FirstOrDefault();
        }
        public async Task<T>  FirstOrDefaultAsync(Expression<Func<T, bool>> predicate=null)
        {
            if (predicate != null)
                return await Task.Run(() => dbContext.Set<T>().Where(predicate).FirstOrDefault());
            return await Task.Run(() => dbContext.Set<T>().FirstOrDefault());
        }
    }
}