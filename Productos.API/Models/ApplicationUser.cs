using Microsoft.AspNetCore.Identity;
namespace Productos.API.Models
{
    // tabla para usuarios
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}
