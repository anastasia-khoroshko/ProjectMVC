using DAL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Repository
{
    public interface ICommentRepository:IRepository<DalComment>
    {
        IEnumerable<DalComment> GetCommentsByPredicate(Expression<Func<DalComment, bool>> f);
    }
}
