using Productos.API.Data;
using Productos.API.Interfaces;
using Productos.API.Models;

namespace Productos.API.Repositorios
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
