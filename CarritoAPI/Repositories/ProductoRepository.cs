using Dapper;
using Microsoft.Data.SqlClient;
using CarritoAPI.Data;
using CarritoAPI.Models;
using CarritoAPI.DTOs;

namespace CarritoAPI.Repositories
{
    public class ProductoRepository
    {
        private readonly CarritoContext _context; // Para escritura (ORM)
        private readonly string _connectionString; // Para lectura (Dapper)

        public ProductoRepository(CarritoContext context, IConfiguration config)
        {
            _context = context;
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        // ==========================================
        // 1. LECTURA MASIVA CON DAPPER (MicroORM)
        // ==========================================
        public async Task<IEnumerable<ProductoLecturaDTO>> ObtenerTodosAsync()
        {
            using (var conexion = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Id, Nombre, Precio, Stock FROM Productos";
                // Dapper mapea directo al DTO de lectura
                return await conexion.QueryAsync<ProductoLecturaDTO>(sql);
            }
        }

        // ==========================================
        // 2. ESCRITURA CON ENTITY FRAMEWORK (ORM)
        // ==========================================
        public async Task<Producto> CrearAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // ==========================================
        // 3. DICCIONARIO (Para búsquedas O(1))
        // ==========================================
        public async Task<Dictionary<int, ProductoLecturaDTO>> ObtenerDiccionarioAsync()
        {
            var productos = await ObtenerTodosAsync();
            return productos.ToDictionary(p => p.Id, p => p);
        }

        // ==========================================
        // 4. LÓGICA DEL CARRITO (Usando HashSet y Dictionary)
        // ==========================================
        public async Task<ResumenCompraDTO> ProcesarCarritoAsync(List<CarritoItemDTO> items)
        {
            // 1. Usamos un HashSet para agrupar los IDs de los productos sin duplicados
            var productosIds = new HashSet<int>(items.Select(i => i.ProductoId));

            // 2. Traemos todos los productos de la BD (usando Dapper)
            var productosEncontrados = await ObtenerTodosAsync();

            // 3. Usamos un Dictionary para buscar los productos por su ID de forma instantánea
            var diccionarioProductos = productosEncontrados.ToDictionary(p => p.Id, p => p);

            decimal totalCompra = 0;

            // 4. Validamos el carrito
            foreach (var item in items)
            {
                // Si el producto no existe en el diccionario, lanzamos error
                if (!diccionarioProductos.TryGetValue(item.ProductoId, out var productoBD))
                    throw new Exception($"El producto con ID {item.ProductoId} no existe.");

                if (productoBD.Stock < item.Cantidad)
                    throw new Exception($"No hay suficiente stock para el producto: {productoBD.Nombre}.");

                // Sumamos al total
                totalCompra += (productoBD.Precio * item.Cantidad);
            }

            // (Opcional) Aquí podrías guardar la venta usando EF Core...

            return new ResumenCompraDTO
            {
                Total = totalCompra,
                CantidadProductosDistintos = productosIds.Count, // El HashSet nos dice cuántos productos distintos hay
                Mensaje = "¡Compra procesada con éxito! (Simulación)"
            };
        }
    }
}
