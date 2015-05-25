using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IProfileService
    {
        ProfileEntity CreateProfile(ProfileEntity profile);
        void UpdateProfile(ProfileEntity profile);
        ProfileEntity GetProfileByUser(int userId);
        void DeleteProfile(int id);
    }
}
