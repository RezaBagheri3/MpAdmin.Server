using System;
using System.Linq;
using System.Linq.Expressions;
using WorkReportApp.DAL;

namespace WorkReportApp.Domain
{
    public interface IRepository<T, TKey> where T : IEntity<TKey>
    {
        T Save(T entity);

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteAll(Expression<Func<T, bool>> predicate);

        IQueryable<T> Get();

        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        T Single();

        T Single(Expression<Func<T, bool>> predicate);

        T SingleOrDefault();

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        T First();

        T First(Expression<Func<T, bool>> predicate);

        T FirstOrDefault();

        T FirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}