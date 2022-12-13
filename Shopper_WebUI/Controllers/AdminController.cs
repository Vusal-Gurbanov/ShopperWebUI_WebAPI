using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopper_BLL.Abstract;
using Shopper_Entity;
using Shopper_WebUI.Identity;
using Shopper_WebUI.Models;
using System.Security.Cryptography.Xml;

namespace Shopper_WebUI.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(IProductService productService,ICategoryService categoryService,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult ProductList()
        {
            ProductListModel model = new ProductListModel()
            {
                Products = _productService.GetAll()
            };
            return View(model);
        }

        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model,List<IFormFile> files)
        {
            ModelState.Remove("Images");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description
            };

            if(files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using(var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); //await CopyToAsync => async Task<ActionResult>
                    }
                }
            }

            _productService.Create(entity);

            return RedirectToAction("ProductList");
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productService.GetByIdWithCategories(id.Value);

            if (product == null) 
            {
                return BadRequest();
            }

            var entity = new ProductModel()
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Images = product.Images,
                Id = product.Id,
                SelectedCategories=product.ProductCategories.Select(i=>i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, List<IFormFile> files, int[] categoryIds)
        {
            var entity = _productService.GetById(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            //entity.Images.Clear();
            if (files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream); //await CopyToAsync => async Task<ActionResult>
                    }
                }
            }
            _productService.Update(entity,categoryIds);
            return RedirectToAction("ProductList");
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);

            if(entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories=_categoryService.GetAll()
            });
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };

            _categoryService.Create(entity);

            return RedirectToAction("CategoryList");
        }

        public IActionResult EditCategory(int? id)
        {
            var entity = _categoryService.GetByIdWithProducts(id.Value);

            return View(new CategoryModel()
            {
                Id=entity.Id,
                Name=entity.Name,
                Products=entity.ProductCategories.Select(i=>i.Product).ToList()
            });
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return RedirectToAction("/admin/categories/" + categoryId);
        }

        public async Task<IActionResult> UserList()
        {
            List<ApplicationUser> userList = _userManager.Users.ToList();
            List<AdminUserModel> adminList = new List<AdminUserModel>();

            foreach (ApplicationUser item in userList)
            {
                AdminUserModel user = new AdminUserModel();
                user.FullName = item.FullName;
                user.Username = item.UserName;
                user.EmailConfirmed = item.EmailConfirmed;
                user.Email = item.Email;
                user.IsAdmin = await _userManager.IsInRoleAsync(item, "admin");

                adminList.Add(user);
            }

            return View(adminList);
        }

        public async Task<IActionResult> UserEdit(string Email)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(Email);
            if (entity == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce hesap oluşturulmamıştır.");
                return View();
            }

            AdminUserModel user = new AdminUserModel();
            user.FullName = entity.FullName;
            user.Username = entity.UserName;
            user.EmailConfirmed = entity.EmailConfirmed;
            user.Email = entity.Email;
            user.IsAdmin = await _userManager.IsInRoleAsync(entity, "admin");

            return View(user);

        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(AdminUserModel model)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(model.Email);
            if (entity == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce hesap oluşturulmamıştır.");
                return View();
            }

            entity.FullName = model.FullName;
            entity.EmailConfirmed = model.EmailConfirmed;
            entity.Email = model.Email;
            if (model.IsAdmin)
            {
                await _userManager.AddToRoleAsync(entity, "admin");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(entity, "admin");
            }

            await _userManager.UpdateAsync(entity);
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(string Email)
        {
            ApplicationUser entity = await _userManager.FindByEmailAsync(Email);
            if (entity != null)
            {
                await _userManager.DeleteAsync(entity);
                return RedirectToAction("UserList");
            }

            ModelState.AddModelError("", "Silme işlemi başarısız!!");
            return View(entity);
        }

        
    }
}
