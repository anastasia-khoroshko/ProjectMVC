using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly DbContext Context;
        private IDictionary<Type, object> repositories;
        public UnitOfWork(DbContext context)
        {
            Context = context;
            repositories = new Dictionary<Type, object>();
            RegisterRepository(new UserRepository(Context));
            RegisterRepository(new RoleRepository(Context));
            RegisterRepository(new PostRepository(Context));
            RegisterRepository(new CategoryRepository(Context));
            RegisterRepository(new CommentRepository(Context));
            RegisterRepository(new ProfileRepository(Context));
            RegisterRepository(new VoteRepository(Context));            
        }

        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            return (IRepository<T>)repositories[typeof(T)];
        }
        private void RegisterRepository<T>(IRepository<T> repository) where T : IEntity
        {
            repositories[typeof(T)] = repository;
        }
        public void Commit()
        {
            if (Context != null)
            {
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
