using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> List()
        {
            var t = await _context
                .Products
                .ToListAsync();
            return t;
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
            => await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody]Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
