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

namespace SiteFileStorage.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IVoteService _voteService;
        private readonly ICommentService _commentService;
        public FilesController(IUserService userService, IPostService postService, IVoteService voteService,
            ICommentService commentService)
        {
            _userService = userService;
            _postService = postService;
            _voteService = voteService;
            _commentService = commentService;
        }
        [HttpGet]
        public ActionResult UploadFiles()
        {
            try
            {
                var model = _postService.GetAllPosts().Select(post => new FileViewModel()
                {
                    Id = post.Id,
                    Name = post.Name
                });
                return View(model);
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

        public ActionResult CreatePost(string file_name, string file_type, string file_size, string name, string desсription)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return RedirectToAction("NotFound", "Home");
                }
                else
                {
                    var user = _userService.GetUserByPredicate(u => u.Email == HttpContext.User.Identity.Name);
                    var post = new PostEntity()
                    {
                        Name = name,
                        Description = desсription,
                        FileName = file_name,
                        FileSize = Convert.ToInt32(file_size),
                        FileType = file_type,
                        UserId = user.Id
                    };
                    _postService.CreatePost(post);
                }
                return RedirectToAction("UploadFiles", "Files");
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
            string path = @"D:\FileStorage\SiteFileStorage\UploadFiles\" + name;
            return File(path, type, name);
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
                return Json(Name, Description);
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
            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        public JsonResult SaveRating(int postID, int rate)
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
            return Json(newScore);

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
                return Json(new NewCommentModel()
                {
                    Id = newComment.Id,
                    Text = newComment.Text,
                    UserName =user.Login,
                    UserEmail =user.Email
                });
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
