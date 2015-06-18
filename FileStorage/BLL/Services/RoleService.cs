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
        public RoleService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public RoleEntity CreateRole(RoleEntity role)
        {
            try
            {
                uow.GetRepository<DalRole>().Create(role.ToDalRole());
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
                return uow.GetRepository<DalRole>().GetAll().Select(role => role.ToBLLRole());
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
                DalRole correctRole = uow.GetRepository<DalRole>().GetByPredicate(lambda);
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
                DalRole role = uow.GetRepository<DalRole>().GetByPredicate(x => x.Id == id);
                if (role == null)
                {
                    throw new Exception();
                }
                uow.GetRepository<DalRole>().Delete(id);
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
                uow.GetRepository<DalRole>().Update(role.ToDalRole());
                uow.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception();
            }
        }
    }
}
