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
        public PostRepository(IUnitOfWork uow)
        {
            context = uow.Context;
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
                UserId = p.UserId
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
                    UserId = post.UserId
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
                        listPosts.Add(new DalPost()
                        {
                Id = post.Id,
                Name = post.Name,
                Description = post.Description,
                FileName = post.FileName,
                FileType = post.FileType,
                FileSize = post.FileSize,
                UserId = post.UserId
                        });
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
                UserId = entity.UserId
            };
            try
            {
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
                    var oldPost = context.Entry(post);
                    oldPost.CurrentValues.SetValues(new Post()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        FileName = entity.FileName,
                        FileType = entity.FileType,
                        FileSize = entity.FileSize,
                        UserId = entity.UserId
                    });
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
