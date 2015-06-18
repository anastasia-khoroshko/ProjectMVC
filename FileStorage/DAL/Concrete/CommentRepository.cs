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
    public class CommentRepository:ICommentRepository
    {
        private readonly DbContext context;
        public CommentRepository(DbContext uow)
        {
            context = uow;
        }
        public IEnumerable<DalComment> GetAll()
        {
            throw new NotImplementedException();
        }

        public DalComment GetByPredicate(Expression<Func<DalComment, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalComment entity)
        {
            Comment comment = new Comment()
            {
                Text=entity.Text,
                PostId=entity.PostId,
                UserId = entity.UserId
            };
            try
            {
                context.Set<Comment>().Add(comment);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalComment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int key)
        {
            try
            {
                var entity = context.Set<Comment>().FirstOrDefault(com => com.Id == key);
                if (entity != null)
                {
                    context.Set<Comment>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<DalComment> GetCommentsByPredicate(Expression<Func<DalComment,bool>> f)
        {
            List<DalComment> listComments = new List<DalComment>();
            Expression<Func<Comment, DalComment>> convert =
                com => new DalComment()
                {
                    Id = com.Id,
                    Text=com.Text,
                    PostId=com.PostId,
                    UserId = com.UserId
                };

            var param = Expression.Parameter(typeof(Comment), "comment");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Comment, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                List<Comment> list = context.Set<Comment>().ToList();
                foreach (Comment com in list)
                {
                    if (func(com) == true)
                    {
                        listComments.Add(new DalComment()
                        {
                            Id = com.Id,
                            Text = com.Text,
                            PostId = com.PostId,
                            UserId = com.UserId
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listComments;
        }
    }
}
