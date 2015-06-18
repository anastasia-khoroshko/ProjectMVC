using BLL.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface ICategoryService
    {
        CategoryEntity CreateCategory(CategoryEntity category);
        IEnumerable<CategoryEntity> GetAllCategories();
        CategoryEntity GetCategoryByPredicate(Expression<Func<CategoryEntity, bool>> f);
        void DeleteCategory(int id);
        void UpdateCategory(CategoryEntity category);
    }
}
