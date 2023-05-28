using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using ProductManagementLambda.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementLambda.Controllers;

/// <summary>
/// API controller for products management in License Management Service.
/// </summary>
[ApiController]
[Route("products-api/products")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductManagementService _productManagementService;
    private readonly ILogger<ProductController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="productManagementService"><see cref="IProductManagementService"/></param>
    /// <param name="logger">Logger instance.</param>
    public ProductController(IProductManagementService productManagementService, ILogger<ProductController> logger)
    {
        _productManagementService = productManagementService;
        _logger = logger;
    }

    /// <summary>
    /// Gets product by ID.
    /// </summary>
    /// <param name="id">Product's unique identifier.</param>
    /// <returns>Product definition.</returns>
    /// Example url call:
    /// 
    /// GET <code>license-management/products-api/products?id=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully returned item", typeof(ProductDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> GetProduct([Required, FromQuery]Guid id)
    {
        var product = await _productManagementService.GetProductByIdAsync(id);

        return Ok(product);
    }

    /// <summary>
    /// Creates new product.
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns>Created product definition.</returns>
    /// Example url call:
    /// 
    /// POST <code>license-management/products-api/products</code>
    /// </remarks>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully created item", typeof(ProductDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> CreateProduct([Required, FromBody]ProductDto productDto)
    {
        var product = await _productManagementService.CreateProductAsync(productDto);

        return CreatedAtAction(nameof(_productManagementService.CreateProductAsync), product);
    }

    /// <summary>
    /// Updates an exisiting product.
    /// </summary>
    /// <param name="productDto"></param>
    /// <returns>Updated product definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// PUT <code>license-management/products-api/products</code>
    /// </remarks>
    [HttpPut]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully updated item", typeof(ProductDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> UpdateProduct([Required, FromBody] ProductDto productDto)
    {
        var product = await _productManagementService.UpdateProductAsync(productDto);

        return CreatedAtAction(nameof(_productManagementService.UpdateProductAsync), product);
    }

    /// <summary>
    /// Deletes an existing product.
    /// </summary>
    /// <param name="id">Product unique identifier.</param>
    /// <returns>Deleted product definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// DELETE <code>license-management/products-api/products?id=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpDelete]
    [SwaggerResponse(StatusCodes.Status200OK, "Returned successfully deleted item", typeof(ProductDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> DeleteProduct([Required, FromQuery]Guid id)
    {
        var product = await _productManagementService.DeleteProductAsync(id);

        return Ok(product);
    }
}