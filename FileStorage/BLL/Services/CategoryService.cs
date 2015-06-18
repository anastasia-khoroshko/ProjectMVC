using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using DAL.Interfaces.DTO;
using DAL.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BLL.Mappers;
using System.Data.SqlClient;

namespace BLL.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IUnitOfWork uow;
        public CategoryService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public CategoryEntity CreateCategory(CategoryEntity category)
        {
            try
            {
                uow.GetRepository<DalCategory>().Create(category.ToDalCategory());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
            return category;
        }

        public IEnumerable<CategoryEntity> GetAllCategories()
        {
            try
            {
                return uow.GetRepository<DalCategory>().GetAll().Select(c => c.ToBllCategory());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public CategoryEntity GetCategoryByPredicate(Expression<Func<CategoryEntity, bool>> f)
        {
            Expression<Func<DalCategory,CategoryEntity>> convert =
                c => c.ToBllCategory();
            var param = Expression.Parameter(typeof(DalCategory), "dalCategory");
            var body = Expression.Invoke(f,
                  Expression.Invoke(convert, param));
            var lambda = Expression.Lambda<Func<DalCategory, bool>>(body, param);
            try
            {
                DalCategory category = uow.GetRepository<DalCategory>().GetByPredicate(lambda);
                if (category == null)
                    return null;
                return category.ToBllCategory();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {
                DalCategory category = uow.GetRepository<DalCategory>().GetByPredicate(x => x.Id == id);
                if (category == null)
                {
                    throw new Exception();
                }
                uow.GetRepository<DalCategory>().Delete(id);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            uow.Commit();
        }

        public void UpdateCategory(CategoryEntity category)
        {
            try
            {
                uow.GetRepository<DalCategory>().Update(category.ToDalCategory());
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
