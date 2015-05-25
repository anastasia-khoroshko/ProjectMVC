using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IRoleService
    {
        RoleEntity CreateRole(RoleEntity role);
        IEnumerable<RoleEntity> GetAllRoles();
        RoleEntity GetRoleByPredicate(Expression<Func<RoleEntity, bool>> f);
        void DeleteRole(int id);
        void UpdateRole(RoleEntity role);
    }
}
