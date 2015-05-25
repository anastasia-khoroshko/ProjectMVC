using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SiteFileStorage.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public IUserService userService
        {
            get { return System.Web.Mvc.DependencyResolver.Current.GetService<IUserService>(); }
        }
        public IRoleService roleService
        {
            get { return System.Web.Mvc.DependencyResolver.Current.GetService<IRoleService>(); }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            UserEntity user = userService.GetUserByPredicate(u => u.Login == username);

            if (user == null) return false;

            RoleEntity userRole = roleService.GetRoleByPredicate(r => r.Id == user.RoleId);

            if (userRole != null && userRole.Name == roleName)
            {
                return true;
            }

            return false;
        }
        public override string[] GetRolesForUser(string username)
        {
            var roles = new string[] { };
            var user = userService.GetUserByPredicate(u => u.Email == username);
            if (user == null) return roles;
            var userRole = roleService.GetRoleByPredicate(r => r.Id == user.RoleId);
            if (userRole != null)
            {
                roles = new string[] { userRole.Name };
            }
            return roles;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            var newRole = new RoleEntity() { Name = roleName };
            roleService.CreateRole(newRole);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }


        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }



        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}