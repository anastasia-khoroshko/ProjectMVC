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
        private readonly ICategoryService _categoryService;
        private readonly IVoteService _voteService;
        public HomeController(IUserService userService, IPostService postService, ICategoryService categoryService, 
            IVoteService voteService)
        {
            _userService = userService;
            _postService = postService;
            _categoryService = categoryService;
            _voteService = voteService;
        }
        public ActionResult Index()
        {
            ViewBag.Categories = _categoryService.GetAllCategories();
            var model = _postService.GetPostsByPredicate(p => p.Permit == true).Select(m => new FileViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Categories = m.Categories.Select(c => c.Name).ToList(),
                    Score = (_voteService.GetVotesByPost(p => p.Id == m.Id) != null) ? _voteService.GetVotesByPost(p => p.Id == m.Id).Select(v => v.Score).Sum() / _voteService.GetVotesByPost(p => p.Id == m.Id).Count() : 0
                });
            return View(model);
        }
        public ActionResult Search(string tag)
        {
            try
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                List<FileViewModel> result = new List<FileViewModel>();
                if (!string.IsNullOrEmpty(tag))
                {
                    var listPosts = _postService.GetPostsByPredicate(post => culture.CompareInfo.IndexOf(post.Name, tag, CompareOptions.IgnoreCase) >= 0
                        || (!string.IsNullOrEmpty(post.Description))?culture.CompareInfo.IndexOf(post.Description, tag, CompareOptions.IgnoreCase) >= 0:false);
                    if (listPosts == null || !listPosts.Any())
                        result = null;
                    else
                    {
                        foreach (PostEntity post in listPosts)
                            if(post.Permit==true)
                            {
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
