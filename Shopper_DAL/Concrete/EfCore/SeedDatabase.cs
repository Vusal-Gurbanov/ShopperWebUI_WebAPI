using Microsoft.EntityFrameworkCore;
using Shopper_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopper_DAL.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new DataContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategory);
                }
                context.SaveChanges();
            }
        }

        private static List<Category> Categories = new List<Category>()
        {
            new Category(){Name="Bilgisayar"},
            new Category(){Name="Telefon"},
            new Category(){Name="Elektronik"}
        };

        private static Product[] Products =
     {
            new Product(){ Name="Samsung Note6", Price=15000, Images={new Image(){ImageUrl="1.jpg" },new Image(){ImageUrl="2.jpg" },new Image() { ImageUrl = "3.jpg" },new Image() { ImageUrl = "4.jpg" } },Description="<p>güzel telefon</p>"},

            new Product(){ Name="Samsung Note7", Price=16000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } },Description="<p>güzel telefon</p>"},
            new Product(){ Name="Samsung Note8", Price=17000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } },Description="<p>güzel telefon</p>"},
            new Product(){ Name="Samsung Note9", Price=18000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } } ,Description="<p>güzel telefon</p>"},
            new Product(){ Name="Samsung Note10", Price=19000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } },Description="<p>güzel telefon</p>"},
            new Product(){ Name="Samsung Note11", Price=20000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } },Description="<p>güzel telefon</p>"},
            new Product(){ Name="Samsung Note12", Price=28000, Images={new Image(){ImageUrl="2.jpg" },new Image(){ImageUrl="3.jpg" },new Image() { ImageUrl = "4.jpg" },new Image() { ImageUrl = "5.jpg" } },Description="<p>güzel telefon</p>"}
        };

        private static ProductCategory[] ProductCategory =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[1]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[0]},
            new ProductCategory(){Product=Products[4],Category=Categories[2]},
            new ProductCategory(){Product=Products[5],Category=Categories[1]},
            new ProductCategory(){Product=Products[6],Category=Categories[1]},
            new ProductCategory(){Product=Products[6],Category=Categories[2]},
            new ProductCategory(){Product=Products[6],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[1]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]}
        };
    }
}
