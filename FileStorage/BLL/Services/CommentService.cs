using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using DAL.Interfaces.DTO;
using System.Data.SqlClient;

namespace BLL.Services
{
    public class CommentService:ICommentService
    {
        private readonly IUnitOfWork uow;
        private readonly ICommentRepository commentRepository;
        public CommentService(IUnitOfWork uow, ICommentRepository commentRepository)
        {
            this.uow = uow;
            this.commentRepository = commentRepository;
        }
        public CommentEntity Create(CommentEntity entity)
        {
            try
            {
                commentRepository.Create(entity.ToDalComment());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return entity;
        }

        public void DeleteComment(int key)
        {
            try
            { 
            DalComment post = commentRepository.GetCommentsByPredicate(x => x.Id == key).FirstOrDefault();
            if (post == null)
            {
                throw new Exception();
            }
            commentRepository.Delete(key);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }

        public IEnumerable<CommentEntity> GetByPostId(int postId)
        {
            try
            { 
            var list = commentRepository.GetCommentsByPredicate(x => x.PostId == postId);
            return list.Select(com => new CommentEntity()
                {
                    Id = com.Id,
                    Text = com.Text,
                    PostId = com.PostId,
                    UserId = com.UserId
                });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
