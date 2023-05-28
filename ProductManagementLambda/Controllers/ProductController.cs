using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using ProductManagementLambda.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementLambda.Controllers;

[Route("product-api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductManagementService _productManagementService;
    private readonly ILogger<ProductController> _logger;

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([Required]Guid id)
    {
        var product = await _productManagementService.GetProductByIdAsync(id);

        return Ok(product);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([Required, FromBody]ProductDto productDto)
    {
        var product = await _productManagementService.CreateProductAsync(productDto);

        return CreatedAtAction(nameof(_productManagementService.CreateProductAsync), product);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([Required, FromBody] ProductDto productDto)
    {
        var product = await _productManagementService.UpdateProductAsync(productDto);

        return CreatedAtAction(nameof(_productManagementService.UpdateProductAsync), product);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([Required]Guid id)
    {
        var product = await _productManagementService.DeleteProductAsync(id);

        return Ok(product);
    }
}