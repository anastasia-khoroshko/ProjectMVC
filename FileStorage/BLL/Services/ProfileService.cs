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
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork uow;
        public ProfileService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public ProfileEntity GetProfileByUser(int userId)
        {
            try
            {
                var profile = ((IProfileRepository)uow.GetRepository<DalProfile>()).GetProfileByUser(userId);
            if (profile == null)
                return null;
            else
                return profile.ToBLLProfile();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public ProfileEntity CreateProfile(ProfileEntity profile)
        {
            try
            {
                uow.GetRepository<DalProfile>().Create(profile.ToDalProfile());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return profile;
        }

        public void UpdateProfile(ProfileEntity profile)
        {

            try
            {
                uow.GetRepository<DalProfile>().Update(profile.ToDalProfile());
                uow.Commit();
            }
            catch (Exception exception)
            {
                throw new Exception();
            }
        }




        public void DeleteProfile(int id)
        {
            try
            {
                DalProfile profile = uow.GetRepository<DalProfile>().GetByPredicate(x => x.Id == id);
                if (profile == null)
                {
                    throw new Exception();
                }
                uow.GetRepository<DalProfile>().Delete(id);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }
    }
}
