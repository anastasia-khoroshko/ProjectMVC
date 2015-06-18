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
    public class CategoryRepository:IRepository<DalCategory>
    {
        private readonly DbContext context;
        public CategoryRepository(DbContext uow)
        {
            context = uow;
        }
        public IEnumerable<DalCategory> GetAll()
        {
            try
            {
                return context.Set<Category>().Select(p => new DalCategory()
                {
                    Id = p.Id,
                    Name = p.Name
                });
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DalCategory GetByPredicate(Expression<Func<DalCategory, bool>> f)
        {
            DalCategory category = null;
            Expression<Func<Category, DalCategory>> convert =
                c => new DalCategory()
                {
                    Id = c.Id,
                    Name = c.Name
                };
            var param = Expression.Parameter(typeof(Category), "category");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<Category, bool>>(body, param);
            var func = lambda.Compile();
            try
            {
                IEnumerable<Category> list = context.Set<Category>();
                foreach (Category c in list)
                {
                    if (func(c) == true)
                    {
                        category = new DalCategory()
                        {
                            Id = c.Id,
                            Name = c.Name
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return category;
        }

        public void Create(DalCategory entity)
        {
            Category category = new Category()
            {
                Name = entity.Name
            };
            try
            {
                context.Set<Category>().Add(category);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Update(DalCategory entity)
        {
            try
            {
                var category = context.Set<Category>().Find(entity.Id);
                if (category != null)
                {
                    var oldCategory = context.Entry(category);
                    oldCategory.CurrentValues.SetValues(new Category()
                    {
                        Id = entity.Id,
                        Name = entity.Name                        
                    });
                    oldCategory.State = EntityState.Modified;
                }
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
                var entity = context.Set<Category>().FirstOrDefault(category => category.Id == key);
                if (entity != null)
                {
                    context.Set<Category>().Remove(entity);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
