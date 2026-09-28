namespace Productos.API.Dtos
{
    public class PaginacionResult<TEntity>
    {
        public List<TEntity> Result { get; set; }

        public int Total { get; set; }
    }
}
