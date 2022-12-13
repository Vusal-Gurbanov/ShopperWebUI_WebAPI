using Shopper_Entity;

namespace Shopper_WebUI.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }     // Toplam Ürün Sayısı
        public int ItemsPerPage { get; set; }   // Her Sayfada Kaç Ürün 
        public int CurrentPage { get; set; }    // Hangi Sayfadayız
        public string CurrentCategory { get; set; } // Hangi Category 

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); // 10/3=> 3.3333=>4  1,2,3,4
        }
    }
    public class ProductListModel
    {
        public List<Product> Products { get; set; }
        public PageInfo PageInfo { get; set; } 
    }
}
