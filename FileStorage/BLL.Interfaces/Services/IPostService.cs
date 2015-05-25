using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IPostService
    {
        PostEntity CreatePost(PostEntity post);
        IEnumerable<PostEntity> GetAllPosts();
        PostEntity GetByPredicate(Expression<Func<PostEntity, bool>> f);
        IEnumerable<PostEntity> GetPostsByPredicate(Expression<Func<PostEntity, bool>> f);
        void DeletePost(int key);
        void UpdatePost(PostEntity entity);
    }
}
