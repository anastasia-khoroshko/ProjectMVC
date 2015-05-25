using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IUserService
    {
        UserEntity CreateUser(UserEntity user);
        IEnumerable<UserEntity> GetAllUsers();
        UserEntity GetUserByPredicate(Expression<Func<UserEntity, bool>> f);
        void DeleteUser(int id);
        void UpdateUser(UserEntity user);
    }
}
