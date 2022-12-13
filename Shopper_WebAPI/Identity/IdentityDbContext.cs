using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shopper_WebAPI.Identity
{
    public class IdentityDbContext:IdentityDbContext<User>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext>options):base(options)
        {

        }        
    }
}
