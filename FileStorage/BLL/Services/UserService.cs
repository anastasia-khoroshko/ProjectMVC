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
        private readonly IRepository<DalUser> userRepository;
        private readonly IRepository<DalRole> roleRepository;
        public UserService(IUnitOfWork uow, IRepository<DalUser> userRepository, IRepository<DalRole> roleRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }
        public UserEntity CreateUser(UserEntity user)
        {
            try
            {
                userRepository.Create(user.ToDalUser());
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
            return userRepository.GetAll().Select(user => user.ToBllUser());
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
            DalUser user = userRepository.GetByPredicate(x => x.Id == id);
            if (user == null)
            {
                throw new Exception();
            }
            userRepository.Delete(id);
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
                userRepository.Update(user.ToDalUser());
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
                DalUser correctUser = userRepository.GetByPredicate(lambda);
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
