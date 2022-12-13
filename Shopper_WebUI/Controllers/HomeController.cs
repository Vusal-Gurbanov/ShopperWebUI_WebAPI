using Microsoft.AspNetCore.Mvc;
using Shopper_BLL.Abstract;
using Shopper_WebUI.Models;

namespace Shopper_WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            //ProductListModel model = new ProductListModel();
            //model.Products = _productService.GetAll();


            //return View(model);

            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }
    }
}