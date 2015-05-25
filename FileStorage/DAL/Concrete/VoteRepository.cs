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
    public class VoteRepository : IVoteRepository
    {
        private readonly DbContext context;
        public VoteRepository(IUnitOfWork uow)
        {
            context = uow.Context;
        }
        public IEnumerable<DalVote> GetVotesByPredicate(Expression<Func<DalVote, bool>> f)
        {
            List<DalVote> listVotes = new List<DalVote>();
            Expression<Func<Vote, DalVote>> convert =
                vote => new DalVote
                {
                    Id = vote.Id,
                    PostId = vote.PostId,
                    Score = vote.Score,
                    UserId=vote.UserId
                };

            var param = Expression.Parameter(typeof(Vote), "vote");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Vote, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                IEnumerable<Vote> list = context.Set<Vote>();
                foreach (Vote vote in list)
                {
                    if (func(vote) == true)
                    {
                        listVotes.Add(new DalVote()
                        {
                            Id = vote.Id,
                            PostId = vote.PostId,
                            Score = vote.Score,
                            UserId = vote.UserId
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listVotes;
        }

        public IEnumerable<DalVote> GetAll()
        {
            throw new NotImplementedException();
        }

        public DalVote GetByPredicate(Expression<Func<DalVote, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalVote entity)
        {
            Vote vote = new Vote()
            {
                PostId = entity.PostId,
                Score = entity.Score,
                UserId=entity.UserId
            };
            try
            {
                context.Set<Vote>().Add(vote);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalVote entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int key)
        {
            try
            {
                var entity = context.Set<Vote>().FirstOrDefault(vote => vote.Id == key);
                if (entity != null)
                {
                    context.Set<Vote>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
