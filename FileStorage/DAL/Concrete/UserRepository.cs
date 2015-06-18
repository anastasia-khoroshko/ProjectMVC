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
    public class UserRepository : IRepository<DalUser>
    {
        private readonly DbContext context;
        public UserRepository(DbContext uow)
        {
            context = uow;
        }
        public IEnumerable<DalUser> GetAll()
        {
            try
            {
                return context.Set<User>().Select(user =>new DalUser()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                CreatedDate = user.CreatedDate,
                RoleId = user.RoleId
            });
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> f)
        {
            DalUser correctUser = null;
            Expression<Func<User, DalUser>> convert =
                user => new DalUser()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    Password = user.Password,
                    CreatedDate = user.CreatedDate,
                    RoleId = user.RoleId
                }; 
            var param = Expression.Parameter(typeof(User), "user");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<User, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                IEnumerable<User> list = context.Set<User>();
                foreach (User u in list)
                {
                    if (func(u) == true)
                    {
                        correctUser = new DalUser()
                        {
                            Id = u.Id,
                            Login = u.Login,
                            Email = u.Email,
                            Password = u.Password,
                            CreatedDate = u.CreatedDate,
                            RoleId = u.RoleId
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return correctUser;
        }

        public void Create(DalUser entity)
        {
            User user = new User()
            {
                Login = entity.Login,
                Password = entity.Password,
                Email = entity.Email,
                CreatedDate = entity.CreatedDate,
                RoleId = entity.RoleId
            };
            try
            {
                context.Set<User>().Add(user);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalUser entity)
        {
            try
            {
                var user = context.Set<User>().Find(entity.Id);
                if (user != null)
                {
                    var oldUser = context.Entry(user);
                    oldUser.CurrentValues.SetValues(new User()
                        {
                            Id = entity.Id,
                            Login = entity.Login,
                            Password = entity.Password,
                            Email = entity.Email,
                            CreatedDate = entity.CreatedDate,
                            RoleId = entity.RoleId
                        });
                    oldUser.State = EntityState.Modified;
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
                var entity = context.Set<User>().FirstOrDefault(user => user.Id == key);
                if (entity != null)
                {
                    context.Set<User>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

    }
}
