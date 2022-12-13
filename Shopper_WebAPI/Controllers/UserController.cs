using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopper_WebAPI.Identity;

namespace Shopper_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<User> _userManager;        
        public UserController(UserManager<User> userManager) //Injection
        {
            _userManager = userManager;
        }

        //API => GET, GET{id}, POST, PUT{id}, DELETE 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }

            return await _userManager.Users.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
               

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(Kullanici model)
        {
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            await _userManager.CreateAsync(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(string id, User model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var user = await _userManager.Users.Where(i => i.Id == id).FirstOrDefaultAsync();

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;


            try
            {
                await _userManager.UpdateAsync(user);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
                return NotFound();

            _userManager.DeleteAsync(user);

            return NoContent();

        }

        private bool UserExists(string id)
        {
            return (_userManager.Users?.Any(i => i.Id == id)).GetValueOrDefault();
        }
    }
}
