using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopper_BLL.Abstract;
using Shopper_BLL.Concrete;
using Shopper_Entity;
using Shopper_WebAPI.Identity;

namespace Shopper_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            if (_categoryService.GetAll() == null)
            {
                return NotFound();
            }

            var liste = _categoryService.GetAll();

            return null;
        }
    }
}
