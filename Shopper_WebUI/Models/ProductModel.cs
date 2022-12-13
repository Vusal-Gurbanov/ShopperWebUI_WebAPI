using Shopper_Entity;
using System.ComponentModel.DataAnnotations;

namespace Shopper_WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Image> Images { get; set; }       
        public List<Category> SelectedCategories { get; set; }       
    }
}
