using DAL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Repository
{
    public interface IVoteRepository : IRepository<DalVote>
    {
        IEnumerable<DalVote> GetVotesByPredicate(Expression<Func<DalVote, bool>> f);
    }
}
