using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NegotiationsAPI.Core.Entities;
using NegotiationsAPI.Core.Interfaces;

namespace NegotiationsAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Employee")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            if (id != product.Id) return BadRequest();

            await _productRepository.UpdateAsync(product);
            return NoContent();
        }

        [Authorize(Roles = "Employee")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
