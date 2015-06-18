using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using SiteFileStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFileStorage.Controllers
{
    public class ModeratorController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ICategoryService _caregoryService;
        public ModeratorController(IUserService userService, IPostService postService, IVoteService voteService,
            ICommentService commentService, ICategoryService categoryService)
        {
            _userService = userService;
            _postService = postService;
            _caregoryService = categoryService;
        }
        public ActionResult Index()
        {
            var posts = _postService.GetAllPosts().Select(m => new ModeratorFileViewModel() 
            {
                Id=m.Id,
                Name=m.Name,
                Description=m.Description,
                FileName=m.FileName,
                FileType=m.FileType,
                Permit=m.Permit,
                Categories=(m.Categories!=null)?m.Categories.Select(c=> c.Name).ToList():null
            });
            
            return View(posts);
        }

        public ActionResult EditFile(int postId)
        {
            var post = _postService.GetByPredicate(p => p.Id == postId);
            if (post != null)
            {
                ViewBag.Categories = _caregoryService.GetAllCategories();
                return View(new ModeratorFileViewModel()
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description=post.Description,
                    FileType=post.FileType,
                    FileName=post.FileName,
                    Permit=post.Permit,
                    Categories=(post.Categories!=null)?post.Categories.Select(c=>c.Name).ToList():null
                });
            }
            else return RedirectToAction("NotFound", "Home");
        }

        public ActionResult SaveChanges(ModeratorFileViewModel model)
        {
            if (model != null)
            {
                var newPost=new PostEntity()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        FileName = model.FileName,
                        FileType = model.FileType,
                        FileSize = _postService.GetByPredicate(p => p.Id == model.Id).FileSize,
                        UserId=_postService.GetByPredicate(p=>p.Id==model.Id).UserId,
                        Permit = model.Permit,
                        Categories = new List<CategoryEntity>()
                    };
                foreach(var category in model.Categories)
                {
                    var existCateg=_caregoryService.GetCategoryByPredicate(c=>c.Name==category);
                    newPost.Categories.Add(existCateg);
                }
                _postService.UpdatePost(newPost);
                if (Request.IsAjaxRequest())
                    return Json(model.Categories);
                else return RedirectToAction("EditFile", new {postId=model.Id });
            }
            else return RedirectToAction("NotFound", "Home");
        }
    }
}
