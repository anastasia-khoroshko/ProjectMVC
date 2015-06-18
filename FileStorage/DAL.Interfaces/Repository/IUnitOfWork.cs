using DAL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        void Commit();
        IRepository<T> GetRepository<T>() where T : IEntity;
    }
}
