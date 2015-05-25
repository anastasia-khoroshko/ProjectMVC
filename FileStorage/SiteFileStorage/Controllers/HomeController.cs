using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using SiteFileStorage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SiteFileStorage.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        public HomeController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string tag)
        {
            try
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                List<FileViewModel> result = new List<FileViewModel>();
                if (!string.IsNullOrEmpty(tag))
                {
                    var listPosts = _postService.GetPostsByPredicate(post => culture.CompareInfo.IndexOf(post.Name, tag, CompareOptions.OrdinalIgnoreCase) >= 0
                        || culture.CompareInfo.IndexOf(post.Description, tag, CompareOptions.OrdinalIgnoreCase) >= 0);
                    if (listPosts == null || !listPosts.Any())
                        result = null;
                    else
                    {
                        foreach (PostEntity post in listPosts)
                            result.Add(new FileViewModel()
                                {
                                    Id = post.Id,
                                    Name = post.Name,
                                    Description = post.Description,
                                    FileName = post.FileName,
                                    FileType = post.FileType,
                                    FileSize = post.FileSize
                                });
                    }
                }
                return View(result);
            }
            catch(Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}
