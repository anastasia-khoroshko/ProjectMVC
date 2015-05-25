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
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<DalUser> userRepository;
        private readonly IRepository<DalRole> roleRepository;
        public RoleService(IUnitOfWork uow, IRepository<DalUser> userRepository, IRepository<DalRole> roleRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }
        public RoleEntity CreateRole(RoleEntity role)
        {
            try
            { 
            roleRepository.Create(role.ToDalRole());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return role;
        }

        public IEnumerable<RoleEntity> GetAllRoles()
        {
            try
            { 
            return roleRepository.GetAll().Select(role => role.ToBLLRole());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public RoleEntity GetRoleByPredicate(Expression<Func<RoleEntity, bool>> f)
        {
            Expression<Func<DalRole, RoleEntity>> convert =
               role => role.ToBLLRole();
            var param = Expression.Parameter(typeof(DalRole), "dalRole");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<DalRole, bool>>(body, param);
            try
            { 
            DalRole correctRole = roleRepository.GetByPredicate(lambda);
            if (correctRole == null)
                return null;
            return correctRole.ToBLLRole();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteRole(int id)
        {
            try
            {
                DalRole role = roleRepository.GetByPredicate(x => x.Id == id);
                if (role == null)
                {
                    throw new Exception();
                }
                roleRepository.Delete(id);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }

        public void UpdateRole(RoleEntity role)
        {
            try
            {
                roleRepository.Update(role.ToDalRole());
                uow.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception();
            }
        }
    }
}
