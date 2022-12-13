using Microsoft.AspNetCore.Identity;

namespace Shopper_WebUI.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
