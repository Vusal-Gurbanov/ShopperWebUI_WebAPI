using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_BLL.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        Product GetProductDetails(int id);
        Product Find(Expression<Func<Product, bool>> filter);
        List<Product> GetAll(Expression<Func<Product, bool>> filter = null);       
        List<Product> GetProductsByCategory(string category, int page, int pageSize);       

        void Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int value);
        void Update(Product entity, int[] categoryIds);
    }
}
