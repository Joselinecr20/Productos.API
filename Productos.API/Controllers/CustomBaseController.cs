using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Productos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class CustomBaseController : ControllerBase
    {
        public string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? string.Empty;
        }
    }
}
