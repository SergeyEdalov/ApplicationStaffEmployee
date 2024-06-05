using Identity.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.DAL.IdentityContext;

public class IdentityContext : IdentityDbContext<User, Role, string>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
        
    }
}
