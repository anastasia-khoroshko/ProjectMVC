using BLL.Interfaces.Services;
using SiteFileStorage.Infrastructure;
using SiteFileStorage.Models;
using SiteFileStorage.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SiteFileStorage.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService service)
        {
            _userService = service;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LogOnViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(model.Login, model.Password))
                    {

                        FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("UploadFiles", "Files");
                        }
                    }

                }
                ModelState.AddModelError("", "Wrong login or password");
                return View(model);
            }
            catch(Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (model.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])
                {
                    ModelState.AddModelError("Captcha", "Text from the image in an incorrect");
                    return View(model);
                }

                var anyUser = _userService.GetAllUsers().Any(u => u.Email.Contains(model.Email));
                if (anyUser)
                {
                    ModelState.AddModelError("Email", "User with this email already exists");
                    return View(model);
                }

                if (ModelState.IsValid)
                {
                    MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(model.Login, model.Email, model.Password, "User");

                    if (membershipUser != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error registreting");
                    }
                }
                return View(model);
            }
            catch(Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }
        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] =
                new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString(CultureInfo.InvariantCulture);
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 170, 50, "Helvetica");

            // Change the response headers to output a JPEG image.
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";

            // Write the image to the response stream in JPEG format.
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);

            // Dispose of the CAPTCHA image object.
            ci.Dispose();
            return null;
        }

    }
}
