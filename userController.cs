using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Simulated in-memory data store
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200 },
            new Product { Id = 2, Name = "Phone", Price = 800 }
        };

        // CREATE
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product newProduct)
        {
            newProduct.Id = products.Max(p => p.Id) + 1;
            products.Add(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        // READ ALL
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(products);
        }

        // READ ONE
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");
            return Ok(product);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return Ok(product);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            products.Remove(product);
            return NoContent();
        }

        // Product model
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
