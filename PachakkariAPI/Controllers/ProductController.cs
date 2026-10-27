using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pachakkari.Application.DTOs;
using Pachakkari.Application.Interfaces;
using Pachakkari.Domain.Entities;
using Pachakkari.Infrastructure.Services;

namespace PachakkariAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ICloudinaryServices _cloudinaryservices;
        public ProductController(IProductRepository repository,ICloudinaryServices cloudinaryServices)
        {
            _repository = repository;
            _cloudinaryservices = cloudinaryServices;


        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(
    [FromForm] CreateProductDto request)
        {
            if (request.Image == null || request.Image.Length == 0)
            {
                return BadRequest("Product image is required.");
            }

            var imageUrl =
                await _cloudinaryservices.UploadImageAsync(request.Image);

            var product = new Product
            {
                Title = request.Title,
                Price = request.Price,
                Category = request.Category,
                Description = request.Description,
                Image = imageUrl
            };

            var newProduct = await _repository.AddAsync(product);

            return Ok(newProduct);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromForm] CreateProductDto request)
        {
            var existingProduct = await _repository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            // Upload a new image only if the admin selects one
            if (request.Image != null && request.Image.Length > 0)
            {
                var imageUrl =
                    await _cloudinaryservices.UploadImageAsync(request.Image);

                existingProduct.Image = imageUrl;
            }

            // Update product details
            existingProduct.Title = request.Title;
            existingProduct.Price = request.Price;
            existingProduct.Category = request.Category;
            existingProduct.Description = request.Description;

            var updatedProduct =
                await _repository.UpdateAsync(id, existingProduct);

            if (updatedProduct == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(updatedProduct);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }





    }
}
