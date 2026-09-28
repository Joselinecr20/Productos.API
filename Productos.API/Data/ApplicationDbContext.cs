using Productos.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Productos.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //la llave primaria de mi Producto
            builder.Entity<Producto>().HasKey(c => c.Id);
            builder.Entity<Producto>().Property(p => p.Precio).HasPrecision(18, 2);
        }

        public DbSet<Producto> Productos { get; set; }

    }
}
