using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using System.Data.SqlClient;

namespace BLL.Services
{
    public class VoteService : IVoteService
    {

        private readonly IUnitOfWork uow;
        private readonly IVoteRepository voteRepository;
        public VoteService(IUnitOfWork uow, IVoteRepository voteRepository)
        {
            this.uow = uow;
            this.voteRepository = voteRepository;
        }
        public VoteEntity CreateVote(VoteEntity vote)
        {
            try
            {
                voteRepository.Create(vote.ToDalVote());
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return vote;
        }

        public IEnumerable<VoteEntity> GetVotesByPost(Expression<Func<VoteEntity, bool>> f)
        {
            Expression<Func<DalVote, VoteEntity>> convert =
                vote => vote.ToBLLVote();
            var param = Expression.Parameter(typeof(DalVote), "dalVote");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<DalVote, bool>>(body, param);
            try
            {
                IEnumerable<DalVote> listVotes = voteRepository.GetVotesByPredicate(lambda);

                if (listVotes.Count() == 0)
                    return null;
                return listVotes.Select(v => v.ToBLLVote());
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
                DalVote vote = voteRepository.GetVotesByPredicate(v => v.Id == key).FirstOrDefault();
                if (vote == null)
                {
                    throw new Exception();
                }
                voteRepository.Delete(key);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }
    }
}
