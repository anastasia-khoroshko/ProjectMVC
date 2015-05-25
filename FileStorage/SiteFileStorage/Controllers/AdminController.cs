using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using SiteFileStorage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SiteFileStorage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public AdminController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
           
        }

        public ActionResult Index()
        {
            try
            { 
            var allUsers = _userService.GetAllUsers();
            return View(allUsers.Select(user => new UserViewModel()
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email,
                CreatedDate = user.CreatedDate.Date,
                Role = _roleService.GetRoleByPredicate(r => r.Id == user.RoleId).Name
            }));
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var user = _userService.GetUserByPredicate(u => u.Id == id);
                return View(new UserViewModel()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    CreatedDate = user.CreatedDate,
                    Role = _roleService.GetRoleByPredicate(r => r.Id == user.RoleId).Name
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(UserViewModel user)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                UserEntity oldUser = _userService.GetUserByPredicate(u => u.Id == user.Id);
                UserEntity editUser = new UserEntity()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email,
                    Password = oldUser.Password,
                    CreatedDate = user.CreatedDate,
                    RoleId = _roleService.GetRoleByPredicate(r => r.Name == user.Role).Id
                };
                _userService.UpdateUser(editUser);
                return RedirectToAction("Index", "Admin");
            }
            else ModelState.AddModelError("", "Invalid data of user");
            return View(user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult Delete(int id)
        {

            try
            {
                _userService.DeleteUser(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult ValidateDate(string createdDate)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(createdDate, out parsedDate))
                return Json("Enter a valid date", JsonRequestBehavior.AllowGet);
            else if (parsedDate > DateTime.Now)
                return Json("Enter date in past", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
