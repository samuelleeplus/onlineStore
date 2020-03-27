using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace OnlineStore.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        bool Add(T entity);
        bool Remove(T entity);

    }
}
