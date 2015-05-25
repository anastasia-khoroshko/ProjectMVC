using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface ICommentService
    {
        CommentEntity Create(CommentEntity entity);
        void DeleteComment(int key);
        IEnumerable<CommentEntity> GetByPostId(int postId);
    }
}
