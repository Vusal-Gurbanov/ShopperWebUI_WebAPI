using Microsoft.EntityFrameworkCore;
using Shopper_DAL.Abstract;
using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_DAL.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, DataContext>, IProductDal
    {
        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new DataContext())
            {
                var product = context.Products
                    .Include(i=> i.ProductCategories)
                    .FirstOrDefault(i => i.Id == entity.Id);

                if (product != null)
                {
                    context.Images.RemoveRange(context.Images.Where(i => i.ProductId == entity.Id).ToList());

                    product.Price = entity.Price;
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.Images = entity.Images;
                    product.ProductCategories = categoryIds.Select(i => new ProductCategory()
                    {
                        CategoryId = i,
                        ProductId = entity.Id
                    }).ToList();
                }

                context.SaveChanges();
            }
        }

        public new List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (var context = new DataContext())
            {
                return filter == null
                        ? context.Products.Include(i=> i.Images).ToList()
                        : context.Products.Include("Images").Where(filter).ToList();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new DataContext())
            {
               
                var products = context.Products.Include("Images").AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Count();
            }
        }

        //public new Product GetById(int id)
        //{
        //    using (var context = new DataContext())
        //    {
        //        return context.Products.Include(i => i.Images).FirstOrDefault(i=> i.Id==id);
        //    }
        //}

        public Product GetProductDetails(int id)
        {
            using (var context = new DataContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include("Images")
                    .Include("Comments")
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
                    
                      
            }
        }

        public List<Product> GetProductsByCategory(string category,int page,int pageSize)
        {
            using(var context = new DataContext())
            {
                //AsQueryable() => kod yazımını bitirmez oluyor istenildiği durumlarda ekleme yapılabilir.
                var products = context.Products.Include("Images").AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public Product GetByIdWithCategories(int value)
        {
            using (var context=new DataContext())
            {
                return context.Products
                    .Where(i => i.Id == value)
                    .Include(i=>i.Images)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
                
        }

    }
}
