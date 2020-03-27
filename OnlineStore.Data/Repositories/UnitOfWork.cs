using OnlineStore.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool IsDisposed = false;
        public ApplicationDbContext Context { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(Context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing && Context != null)
            {
                Context.Dispose();
            }
            IsDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
