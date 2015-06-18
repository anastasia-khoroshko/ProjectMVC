using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class RoleRepository : IRepository<DalRole>
    {
        private readonly DbContext context;
        public RoleRepository(DbContext uow)
        {
            context = uow;
        }
        public IEnumerable<DalRole> GetAll()
        {
            try
            {
                return context.Set<DalRole>().Select(role => new DalRole()
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

        public DalRole GetByPredicate(Expression<Func<DalRole, bool>> f)
        {
            DalRole correctRole = null;
            Expression<Func<Role, DalRole>> convert =
                role => new DalRole
                {
                    Id = role.Id,
                    Name = role.Name
                };

            var param = Expression.Parameter(typeof(Role), "role");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Role, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                IEnumerable<Role> list = context.Set<Role>();
                foreach (Role r in list)
                {
                    if (func(r) == true)
                    {
                        correctRole = new DalRole()
                        {
                            Id = r.Id,
                            Name = r.Name
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return correctRole;
        }

        public void Create(DalRole entity)
        {
            var role = new Role()
            {
                Name = entity.Name
            };
            try
            {
                context.Set<Role>().Add(role);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalRole entity)
        {
            try
            {
                var role = context.Set<Role>().Find(entity.Id);
                if (role != null)
                {
                    var oldRole = context.Entry(role);
                    oldRole.CurrentValues.SetValues(new Role()
                    {
                        Id = entity.Id,
                        Name = entity.Name
                    });
                    oldRole.State = EntityState.Modified;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Delete(int key)
        {
            try
            {
                var entity = context.Set<Role>().FirstOrDefault(role => role.Id == key);
                if (entity != null)
                {
                    context.Set<Role>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
