using Productos.API.Dtos;
using Productos.API.Interfaces;
using Productos.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Productos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductosController : CustomBaseController
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Creador")]
        public async Task<IActionResult> Create(ProductoDto producto)
        {
            await _productoRepository.CreateAsync(new Producto()
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock
            });

            return Ok(producto);
        }

        [HttpGet("{skip}/{take}")]
        [Authorize(Roles = "Visualizador")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var productos = await _productoRepository.GetAsync(skip, take);
            return Ok(productos);
        }
    }
}
