using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopper_BLL.Abstract;
using Shopper_Entity;
using Shopper_WebUI.Identity;
using Shopper_WebUI.Models;

namespace Shopper_WebUI.Controllers
{
    public class CommentController : Controller
    {
        private IProductService _productService;
        private ICommentService _commentService;
        private UserManager<ApplicationUser> _userManager;
        public CommentController(IProductService productService, ICommentService commentService,UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _commentService = commentService;
            _userManager = userManager;
        }
        public async Task<IActionResult> ShowProductComments(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = _productService.GetProductDetails(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            List<CommentModel> comments = new List<CommentModel>();

            if (product.Comments.Count > 0)
            {
                foreach (Comment item in product.Comments)
                {
                    CommentModel cmd = new CommentModel();
                    cmd.Text = item.Text;
                    cmd.Product = item.Product;
                    cmd.CreateOn = item.CreateOn;


                    var user = await _userManager.FindByIdAsync(item.UserId);

                    cmd.Username = user.UserName;
                    cmd.UserId = user.Id;

                    comments.Add(cmd);
                }
            }
          


            return PartialView("_PartialComments", comments);
        }

        [Authorize]
        public IActionResult Edit(int? id, string text) 
        {
            if (id == null)
            {
                return BadRequest();
            }

            Comment comment = _commentService.GetById(id.Value);

            if (comment == null)
                return BadRequest();

            comment.Text = text.Trim('\n').Trim(' ');
            comment.CreateOn = DateTime.Now;

            _commentService.Update(comment);

            return Json(new { result = true });
                      
        }


        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Comment comment = _commentService.GetById(id.Value);

            if(comment == null)
            {
                return NotFound();
            }

            _commentService.Delete(comment);

            return Json(new { result = true });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(int? productid,CommentModel model)
        {
            ModelState.Remove("Username");
            ModelState.Remove("Product");
            if (ModelState.IsValid)
            {
                if(productid == null)
                {
                    return BadRequest();
                }

                Product product = _productService.GetById(productid.Value);

                if (product == null)
                {
                    return NotFound();
                }

                Comment comment = new Comment();
                comment.Text = model.Text;
                comment.ProductId = product.Id;
                comment.UserId = _userManager.GetUserId(User);
                comment.CreateOn = DateTime.Now;

                _commentService.Create(comment);

                return Json(new { result = true });
            }

            return View(model);
        }
    }
}
