using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjModeloIdentity.Areas.Identity.Data;

//No fundo, fundo, se herda de DbContext
//Porque se usar DbContext do Identity, porque o mesmo já definiu o DbSet para todas as tabelas que o Identity irá criar
public class AspNetCoreIdentityContext : IdentityDbContext<IdentityUser>
{
    public AspNetCoreIdentityContext(DbContextOptions<AspNetCoreIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
