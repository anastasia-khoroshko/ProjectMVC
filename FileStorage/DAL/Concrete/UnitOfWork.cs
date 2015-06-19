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
        private readonly IDictionary<Type, object> repositories;
        public UnitOfWork(DbContext context)
        {
            Context = context;
            repositories = new Dictionary<Type, object>();
            Register(new UserRepository(Context));
            Register(new RoleRepository(Context));
            Register(new PostRepository(Context));
            Register(new CategoryRepository(Context));
            Register(new CommentRepository(Context));
            Register(new ProfileRepository(Context));
            Register(new VoteRepository(Context));            
        }

        public IRepository<T> GetRepository<T>() where T : IEntity
        {
            return (IRepository<T>)repositories[typeof(T)];
        }
        private void Register<T>(IRepository<T> repository) where T : IEntity
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
