using DAL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Repository
{
    public interface IProfileRepository : IRepository<DalProfile>
    {
        DalProfile GetProfileByUser(int userId);
    }
}
