using System.ComponentModel.DataAnnotations;

namespace Productos.API.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
    }
}
