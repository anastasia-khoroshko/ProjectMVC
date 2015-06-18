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
        public CommentService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public CommentEntity Create(CommentEntity entity)
        {
            try
            {
                uow.GetRepository<DalComment>().Create(entity.ToDalComment());
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
                DalComment post = ((ICommentRepository)uow.GetRepository<DalComment>()).GetCommentsByPredicate(x => x.Id == key).FirstOrDefault();
            if (post == null)
            {
                throw new Exception();
            }
            uow.GetRepository<DalComment>().Delete(key);
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
                var list = ((ICommentRepository)uow.GetRepository<DalComment>()).GetCommentsByPredicate(x => x.PostId == postId);
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
