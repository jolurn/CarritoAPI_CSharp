using Microsoft.AspNetCore.Mvc;
using CarritoAPI.DTOs;
using CarritoAPI.Models;
using CarritoAPI.Repositories;

namespace CarritoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoRepository _repository;

        public ProductosController(ProductoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/productos (Usa Dapper y DTO de lectura)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productos = await _repository.ObtenerTodosAsync();
            return Ok(productos);
        }

        // POST: api/productos (Usa EF Core y DTO de creación)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoCreateDTO dto)
        {
            // ASP.NET Core intercepta y valida las Data Annotations aquí
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Mapeo Manual: DTO -> Entidad (Diapositiva 3)
            var nuevoProducto = new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Stock = dto.Stock
            };

            var creado = await _repository.CrearAsync(nuevoProducto);
            return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
        }

        // POST: api/Productos/comprar
        [HttpPost("comprar")]
        public async Task<IActionResult> Comprar([FromBody] List<CarritoItemDTO> carrito)
        {
            if (carrito == null || !carrito.Any())
                return BadRequest(new { mensaje = "El carrito está vacío." });

            try
            {
                var resumen = await _repository.ProcesarCarritoAsync(carrito);
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                // Si no hay stock o el producto no existe, devolvemos un 400
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
