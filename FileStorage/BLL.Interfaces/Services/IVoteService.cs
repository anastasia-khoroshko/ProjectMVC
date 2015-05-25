using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IVoteService
    {
        VoteEntity CreateVote(VoteEntity vote);
        IEnumerable<VoteEntity> GetVotesByPost(Expression<Func<VoteEntity, bool>> f);
        void Delete(int key);
    }
}
