using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using System.Linq.Expressions;
using DAL.Interfaces.DTO;
using System.Data.SqlClient;

namespace BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork uow;
        private readonly IPostRepository postRepository;
        public PostService(IUnitOfWork uow, IPostRepository postRepository)
        {
            this.uow = uow;
            this.postRepository = postRepository;
        }
        public PostEntity CreatePost(PostEntity post)
        {
            try
            {
                postRepository.Create(post.ToDalPost());
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return post;
        }
        public IEnumerable<PostEntity> GetAllPosts()
        {
            try
            { 
            return postRepository.GetAll().Select(post => post.ToBLLEntity());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public PostEntity GetByPredicate(Expression<Func<PostEntity, bool>> f)
        {
            try
            {
                return GetPostsByPredicate(f).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PostEntity> GetPostsByPredicate(Expression<Func<PostEntity, bool>> f)
        {
            Expression<Func<DalPost, PostEntity>> convert =
                post => post.ToBLLEntity();
            var param = Expression.Parameter(typeof(DalPost), "dalPost");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<DalPost, bool>>(body, param);
            try
            {
                IEnumerable<DalPost> listPosts = postRepository.GetPostByPredicate(lambda);
                if (listPosts.Count() == 0)
                    return null;
                return listPosts.Select(p => p.ToBLLEntity());
            }
            catch(SqlException ex)
            {
                throw ex;
            }
        }


        public void DeletePost(int key)
        {
            try
            {
                DalPost post = postRepository.GetByPredicate(x => x.Id == key);
                if (post == null)
                {
                    throw new Exception();
                }
                postRepository.Delete(key);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }

        public void UpdatePost(PostEntity entity)
        {
            try
            {
                postRepository.Update(entity.ToDalPost());
                uow.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception();
            }
        }
    }
}
