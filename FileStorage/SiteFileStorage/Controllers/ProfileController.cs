using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using SiteFileStorage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFileStorage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;
        private readonly IPostService _postservice;
        public ProfileController(IProfileService profileService, IUserService userService, IPostService postService)
        {
            _profileService = profileService;
            _userService = userService;
            _postservice = postService;
        }
        public ActionResult Index(string email)
        {
            try
            {
                var user = _userService.GetUserByPredicate(u => u.Email==email);
                var profile = _profileService.GetProfileByUser(user.Id);
                if (profile == null && user.Email == HttpContext.User.Identity.Name)
                    return RedirectToAction("Create", "Profile");
                else if (profile == null)
                    return View("NoProfile");
                IEnumerable<PostEntity> filesCurrentUser = _postservice.GetPostsByPredicate(p => p.UserId == user.Id);
                ProfileViewModel profileView = new ProfileViewModel()
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Age = profile.Age,
                    Login = user.Login,
                    Email = user.Email,
                    Country = profile.Country,
                    City = profile.City
                };
                if (filesCurrentUser == null)
                    profileView.UploadFiles = null;
                else
                    profileView.UploadFiles = filesCurrentUser.Select(f => new FileViewModel()
                    {
                        Id = f.Id,
                        Name = f.Name,
                    });
                return View(profileView);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProfileViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name);
                    var profile = new ProfileEntity()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Age = model.Age,
                        Country = model.Country,
                        City = model.City,
                        UserId = user.Id
                    };
                    _profileService.CreateProfile(profile);
                }
                return RedirectToAction("Index", "Profile", new { email=HttpContext.User.Identity.Name});
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        [HttpGet]
        public ActionResult Edit()
        {
            try
            {
                var currentUser = _userService.GetUserByPredicate(user => user.Email == HttpContext.User.Identity.Name);
                var profile = _profileService.GetProfileByUser(currentUser.Id);
                if (profile == null)
                    return RedirectToAction("Create", "Profile");
                ProfileViewModel profileView = new ProfileViewModel()
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Age = profile.Age,
                    Login = currentUser.Login,
                    Email = currentUser.Email,
                    Country = profile.Country,
                    City = profile.City
                };
                return View(profileView);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel model)
        {
            UserEntity user = new UserEntity() ;
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        user= _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name);
                        var profile = new ProfileEntity()
                        {
                            Id = model.Id,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Age = model.Age,
                            Country = model.Country,
                            City = model.City,
                            UserId = user.Id
                        };
                        _profileService.UpdateProfile(profile);
                        var newUser = new UserEntity()
                        {
                            Id=user.Id,
                            Login = model.Login,
                            Email = user.Email,
                            Password = user.Password,
                            CreatedDate = user.CreatedDate,
                            RoleId = user.RoleId
                        };
                        _userService.UpdateUser(newUser);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error in editting your profile!");
                        return View(model);
                    }
                }

                return RedirectToAction("Index", "Profile", new { email=user.Email});
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

    }
}
