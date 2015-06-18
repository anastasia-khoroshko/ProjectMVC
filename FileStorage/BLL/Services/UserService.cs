using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using System.Data.SqlClient;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        public UserService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public UserEntity CreateUser(UserEntity user)
        {
            try
            {
                uow.GetRepository<DalUser>().Create(user.ToDalUser());
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return user;
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            try
            {
                return uow.GetRepository<DalUser>().GetAll().Select(user => user.ToBllUser());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                DalUser user = uow.GetRepository<DalUser>().GetByPredicate(x => x.Id == id);
            if (user == null)
            {
                throw new Exception();
            }
            uow.GetRepository<DalUser>().Delete(id);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }

        public void UpdateUser(UserEntity user)
        {
            try
            {
                uow.GetRepository<DalUser>().Update(user.ToDalUser());
                uow.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception();
            }
        }


        public UserEntity GetUserByPredicate(Expression<Func<UserEntity, bool>> f)
        {
            Expression<Func<DalUser, UserEntity>> convert =
                user => user.ToBllUser();
            var param = Expression.Parameter(typeof(DalUser), "dalUser");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<DalUser, bool>>(body, param);
            try
            {
                DalUser correctUser = uow.GetRepository<DalUser>().GetByPredicate(lambda);
                if (correctUser == null)
                    return null;
                return correctUser.ToBllUser();
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }

    }
}
