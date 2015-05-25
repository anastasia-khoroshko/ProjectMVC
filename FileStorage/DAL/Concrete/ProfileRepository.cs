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
    public class ProfileRepository : IProfileRepository
    {
        private readonly DbContext context;
        public ProfileRepository(IUnitOfWork uow)
        {
            context = uow.Context;
        }

        public void Create(DalProfile entity)
        {
            try
            {
                Profile profile = new Profile()
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Age = entity.Age,
                    Country = entity.Country,
                    City = entity.City,
                    UserId = entity.UserId
                };
                context.Set<Profile>().Add(profile);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalProfile entity)
        {
            try
            {
                var profile = context.Set<Profile>().Find(entity.Id);
                if (profile != null)
                {
                    var oldProfile = context.Entry(profile);
                    oldProfile.CurrentValues.SetValues(new Profile()
                    {
                        Id = profile.Id,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Age = entity.Age,
                        Country = entity.Country,
                        City = entity.City,
                        UserId = profile.UserId
                    });
                    oldProfile.State = EntityState.Modified;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DalProfile GetProfileByUser(int userId)
        {
            try
            {
                var profile = context.Set<Profile>().Where(pr => pr.UserId == userId).FirstOrDefault();
                if (profile == null)
                    return null;
                return new DalProfile()
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Age = profile.Age,
                    Country = profile.Country,
                    City = profile.City,
                    UserId = profile.UserId
                };
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public IEnumerable<DalProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public DalProfile GetByPredicate(Expression<Func<DalProfile, bool>> f)
        {
            DalProfile correctProfile = null;
            Expression<Func<Profile, DalProfile>> convert =
                profile => new DalProfile()
                {
                    Id = profile.Id,
                    FirstName=profile.FirstName,
                    LastName=profile.LastName,
                    Age=profile.Age,
                    Country=profile.Country,
                    City=profile.City,
                    UserId=profile.UserId
                };

            var param = Expression.Parameter(typeof(Profile), "profile");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Profile, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                IEnumerable<Profile> list = context.Set<Profile>();
                foreach (Profile p in list)
                {
                    if (func(p) == true)
                    {
                        correctProfile = new DalProfile()
                        {
                            Id = p.Id,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            Age = p.Age,
                            Country = p.Country,
                            City = p.City,
                            UserId = p.UserId
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return correctProfile;
        }



        public void Delete(int key)
        {
            try
            {
                var entity = context.Set<Profile>().FirstOrDefault(profile => profile.Id == key);
                if (entity != null)
                {
                    context.Set<Profile>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
