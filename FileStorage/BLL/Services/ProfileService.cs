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
        private readonly IProfileRepository profileRepository;
        public ProfileService(IUnitOfWork uow, IProfileRepository profileRepository)
        {
            this.uow = uow;
            this.profileRepository = profileRepository;
        }
        public ProfileEntity GetProfileByUser(int userId)
        {
            try
            { 
            var profile = profileRepository.GetProfileByUser(userId);
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
            profileRepository.Create(profile.ToDalProfile());
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
                profileRepository.Update(profile.ToDalProfile());
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
                DalProfile profile = profileRepository.GetByPredicate(x => x.Id == id);
                if (profile == null)
                {
                    throw new Exception();
                }
                profileRepository.Delete(id);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }
    }
}
