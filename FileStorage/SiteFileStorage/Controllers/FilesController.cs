using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using SiteFileStorage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Helpers;

namespace SiteFileStorage.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IVoteService _voteService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _caregoryService;
        public FilesController(IUserService userService, IPostService postService, IVoteService voteService,
            ICommentService commentService, ICategoryService categoryService)
        {
            _userService = userService;
            _postService = postService;
            _voteService = voteService;
            _commentService = commentService;
            _caregoryService = categoryService;
        }
        [HttpGet]
        public ActionResult UploadFiles(int? page)
        {
           IEnumerable<FileViewModel> model = new List<FileViewModel>();
            try
            {
                var posts = _postService.GetPostsByPredicate(post => post.Permit == true);
                if(posts!=null)
                model=posts.Select(post => new FileViewModel()
                {
                    Id = post.Id,
                    Name = post.Name
                });
                ViewBag.Categories = _caregoryService.GetAllCategories();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(model.ToPagedList(pageNumber,pageSize));
            }
            catch (SqlException ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }
        [HttpPost]
        public ContentResult SaveFiles()
        {
            var r = new List<NewFileModel>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                string fullPath = Path.Combine(Server.MapPath("~/UploadFiles"), hpf.FileName);
                hpf.SaveAs(fullPath);

                r.Add(new NewFileModel()
                {
                    Name = hpf.FileName,
                    Size = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + r[0].Size + "\"}", "application/json");
        }

        public ActionResult CreatePost(UploadModelView model)
        {
            PostEntity post;
            try
            {
                if (model==null)
                {
                    return RedirectToAction("NotFound", "Home");
                }
                else
                {
                    var user = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name);
                    post = new PostEntity()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        FileName = model.FileName,
                        FileSize = Convert.ToInt32(model.FileSize),
                        FileType = model.FileType,
                        UserId = user.Id,
                        Permit=false,
                        Categories=new List<CategoryEntity>()
                    };

                    CategoryEntity category;
                    if (model.Categories!=null)
                    {
                        foreach (var elem in model.Categories)
                        {
                            category = _caregoryService.GetCategoryByPredicate(c => c.Name == elem);
                            post.Categories.Add(category);
                        }
                    }
                    _postService.CreatePost(post);
                }
                if (Request.IsAjaxRequest())
                    return Json(post);
                else return RedirectToAction("UploadFiles");
            }
            catch (SqlException ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult ShowPost(int id)
        {
            try
            {
                var post = _postService.GetByPredicate(p => p.Id == id);
                var user = _userService.GetUserByPredicate(u => u.Id == post.UserId);
                var vote = _voteService.GetVotesByPost(v => v.PostId == post.Id);
                double currentScore = vote == null ? 0 : vote.Select(v => v.Score).Sum() / vote.Select(v => v.Score).Count();
                var listRatedUser = vote == null ? null : vote.Select(v => v.UserId);
                var commentsList = _commentService.GetByPostId(id).Select(com => new NewCommentModel()
                {
                    Id = com.Id,
                    Text = com.Text,
                    PostId = com.PostId,
                    UserEmail = _userService.GetUserByPredicate(u => u.Id == com.UserId).Email,
                    UserName = _userService.GetUserByPredicate(u => u.Id == com.UserId).Login
                });
                ViewBag.Comments = commentsList;
                return View(new FileViewModel()
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    FileName = post.FileName,
                    FileType = post.FileType,
                    FileSize = post.FileSize / 1024,
                    UserName = user.Login,
                    UserEmail = user.Email,
                    CurrentUserId = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name).Id,
                    Score = currentScore,
                    RatedUser = listRatedUser
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult FileShow(string name)
        {
            return new CustomFileResult(name);
        }

        public FileResult Download(string name, string type)
        {
            string path = Path.Combine(Server.MapPath("~/UploadFiles"), name);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet,name);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var user = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name);
                var post = _postService.GetByPredicate(p => p.Id == id);
                if (post == null)
                    return RedirectToAction("NotFound", "Home");
                else
                {
                    return View(new FileViewModel()
                    {
                        Id = post.Id,
                        Name = post.Name,
                        Description = post.Description,
                        FileName = post.FileName,
                        FileType = post.FileType,
                        FileSize = post.FileSize,
                        UserName = user.Login,
                        UserEmail = user.Email
                    });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");

            }
        }
        [HttpPost]
        public ActionResult SaveChanges(string Name, string Description, int Id)
        {
            try
            {
                var oldPost = _postService.GetByPredicate(p => p.Id == Id);
                if (oldPost != null)
                {
                    if (!string.IsNullOrEmpty(Name))
                        _postService.UpdatePost(new PostEntity()
                        {
                            Id = oldPost.Id,
                            Name = Name,
                            Description = Description,
                            FileName = oldPost.FileName,
                            FileSize = oldPost.FileSize,
                            FileType = oldPost.FileType,
                            UserId = oldPost.UserId
                        });
                }
                else
                {
                    return RedirectToAction("NotFound", "Home");

                }
                if(Request.IsAjaxRequest())
                return Json(Name, Description);
                else return RedirectToAction("ShowPost", new { id = Id });
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
                _postService.DeletePost(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return RedirectToAction("Index", "Profile", new {email=HttpContext.User.Identity.Name });
        }
        [HttpPost]
        public ActionResult SaveRating(int postID, int rate)
        {

            var newVote = new VoteEntity()
            {
                PostId = postID,
                UserId = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name).Id,
                Score = rate
            };
            _voteService.CreateVote(newVote);
            var vote = _voteService.GetVotesByPost(v => v.PostId == postID);
            var listScore = vote.Select(v => v.Score);
            double newScore = listScore.Sum() / listScore.Count();
            if(Request.IsAjaxRequest())
            return Json(newScore);
            else return RedirectToAction("ShowPost", new { id = postID });

        }

        public ActionResult SaveComment(int postId, string comment)
        {
            try
            {
                var post = _postService.GetPostsByPredicate(p => p.Id == postId);
                if (string.IsNullOrEmpty(comment) || post == null)
                    return new HttpStatusCodeResult(202);
                var newComment = new CommentEntity()
                    {
                        Text = comment,
                        PostId = postId,
                        UserId = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name).Id
                    };
                _commentService.Create(newComment);
                var user=_userService.GetUserByPredicate(u => u.Id == newComment.UserId);
                if (Request.IsAjaxRequest())
                    return Json(new NewCommentModel()
                    {
                        Id = newComment.Id,
                        Text = newComment.Text,
                        UserName = user.Login,
                        UserEmail = user.Email
                    });
                else return  RedirectToAction("ShowPost", new { id = postId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
        }

        public ActionResult DeleteComment(int id)
        {
            try
            {
                _commentService.DeleteComment(id);
            }
            catch (Exception ex)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return RedirectToAction("ShowPost", "File");
        }
    }
}
