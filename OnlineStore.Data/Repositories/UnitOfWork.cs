using OnlineStore.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetGenericRepository<T>() where T : class;
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _isDisposed = false;
        public ApplicationDbContext Context { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }
        public IRepository<T> GetGenericRepository<T>() where T : class
        {
            return new Repository<T>(Context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                Context?.Dispose();
            }
            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
