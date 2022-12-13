using Microsoft.AspNetCore.Identity;

namespace Shopper_WebAPI.Identity
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
    }
}
