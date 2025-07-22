using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Models.DTOs;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(ILogger<ProductController> logger, ApplicationDbContext context) : ControllerBase
    {


        private readonly ILogger<ProductController> _logger = logger;
        private readonly ApplicationDbContext _context=context;

        [HttpGet(Name = "GetProduct")]
        [Authorize]
        public IEnumerable<Product> Get()
        {

            return context.Products.ToArray();

        }

        [HttpGet("{id}")]
        public IEnumerable<Product> Get(int id)
        {

            return _context.Products.Where(c => c.Id == id).ToArray();

        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] CreateProductRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                Category = request.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }


            if (!string.IsNullOrEmpty(request.Name))
                product.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                product.Description = request.Description;

            if (request.Price.HasValue)
                product.Price = request.Price.Value;

            if (request.StockQuantity.HasValue)
                product.StockQuantity = request.StockQuantity.Value;

            if (!string.IsNullOrEmpty(request.Category))
                product.Category = request.Category;

            product.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Ok(product);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var p = context.Products.FirstOrDefault(c => c.Id == id);
            if (p != null)
            {
                _context.Products.Remove(p);
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }
    }
}
