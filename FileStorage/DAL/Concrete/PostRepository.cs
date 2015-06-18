using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Concrete
{
    public class PostRepository : IPostRepository
    {
        private readonly DbContext context;
        public PostRepository(DbContext uow)
        {
            context = uow;
        }
        public IEnumerable<DalPost> GetAll()
        {
            try
            {
                return context.Set<Post>().Select(p => new DalPost()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                FileName = p.FileName,
                FileType = p.FileType,
                FileSize = p.FileSize,
                UserId = p.UserId,
                Permit = p.Permit,
                Categories = p.Categories.Select(s => new DalCategory()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DalPost GetByPredicate(Expression<Func<DalPost, bool>> f)
        {
            return GetPostByPredicate(f).FirstOrDefault();
        }

        public IEnumerable<DalPost> GetPostByPredicate(Expression<Func<DalPost, bool>> f)
        {
            List<DalPost> listPosts = new List<DalPost>();
            Expression<Func<Post, DalPost>> convert =
                post => new DalPost()
                {
                    Id = post.Id,
                    Name = post.Name,
                    Description = post.Description,
                    FileName = post.FileName,
                    FileType = post.FileType,
                    FileSize = post.FileSize,
                    UserId = post.UserId,
                    Permit = post.Permit,
                    Categories = post.Categories.Select(s => new DalCategory()
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                };

            var param = Expression.Parameter(typeof(Post), "post");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Post, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                List<Post> list = context.Set<Post>().ToList();
                foreach (Post post in list)
                {
                    if (func(post) == true)
                    {
                        var correct=new DalPost()
                        {
                            Id = post.Id,
                            Name = post.Name,
                            Description = post.Description,
                            FileName = post.FileName,
                            FileType = post.FileType,
                            FileSize = post.FileSize,
                            UserId = post.UserId,
                            Permit = post.Permit
                        };
                        if(post.Categories.Count!=0)
                        {
                            correct.Categories = post.Categories.Select(s => new DalCategory()
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).ToList();
                        }
                        listPosts.Add(correct);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listPosts;
        }

        public void Create(DalPost entity)
        {
            Post post = new Post()
            {
                Name = entity.Name,
                Description = entity.Description,
                FileName = entity.FileName,
                FileType = entity.FileType,
                FileSize = entity.FileSize,
                UserId = entity.UserId,
                Permit = entity.Permit
            };
            try
            {
                var list=new List<Category>();
                foreach(var ele in entity.Categories)
                {
                    list.Add(context.Set<Category>().Where(c=>c.Id==ele.Id).FirstOrDefault());
                }
                post.Categories = list;
                context.Set<Post>().Add(post);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalPost entity)
        {
            try
            {
                var post = context.Set<Post>().Find(entity.Id);
                if (post != null)
                {
                    post.Categories = null;
                    context.SaveChanges();
                    var list = new List<Category>();
                    foreach (var ele in entity.Categories)
                    {
                        list.Add(context.Set<Category>().Where(c => c.Id == ele.Id).FirstOrDefault());
                    }
                    post.Categories = list;
                    context.SaveChanges();
                    var oldPost = context.Entry(post);
                    oldPost.CurrentValues.SetValues(new Post()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        FileName = entity.FileName,
                        FileType = entity.FileType,
                        FileSize = entity.FileSize,
                        UserId = entity.UserId,
                        Permit = entity.Permit,
                        Categories=list
                    });
                   
                    oldPost.Entity.Categories = list;
                    oldPost.State = EntityState.Modified;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Delete(int key)
        {
            try
            {
                var entity = context.Set<Post>().FirstOrDefault(post => post.Id == key);
                if (entity != null)
                {
                    context.Set<Post>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


    }
}
