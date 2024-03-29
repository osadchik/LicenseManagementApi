using Common.Entities;
using LicenseManagementLambda.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LicenseManagementLambda.Controllers;

/// <summary>
/// API controller for product entitlement management in License Management Service.
/// </summary>
[ApiController]
[Route("license-api/entitlements")]
[Produces("application/json")]
public class ProductEntitlementController : ControllerBase
{
    private readonly IProductEntitlementManagementService _productEntitlementManagementService;
    private readonly ILogger<ProductEntitlementController> _logger;

    public ProductEntitlementController(IProductEntitlementManagementService productEntitlementManagementService, ILogger<ProductEntitlementController> logger)
    {
        _productEntitlementManagementService = productEntitlementManagementService;
        _logger = logger;
    }

    /// <summary>
    /// Gets product entitlement by ID.
    /// </summary>
    /// <param name="entitlementId">Product's entitlement unique identifier.</param>
    /// <returns>Product entitlement entity.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// GET <code>license-management/license-api/entitlements?entitlementId=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully returned item", typeof(ProductEntitlementDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> GetEntitlement([Required, FromQuery] Guid entitlementId)
    {
        var entitlement = await _productEntitlementManagementService.GetEntitlementByIdAsync(entitlementId);

        return Ok(entitlement);
    }

    /// <summary>
    /// Creates new product entitlement.
    /// </summary>
    /// <param name="licenseId">License unique identifier.</param>
    /// <param name="userId">User's unique identifier.</param>
    /// <returns>Created product entitlement definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// POST <code>license-management/license-api/entitlements?licenseId={id}&amp;userId={id}</code>
    /// </remarks>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully created item", typeof(ProductEntitlementDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> CreateEntitlement([FromQuery, Required] Guid licenseId, [FromQuery, Required] Guid productId, [FromQuery, Required] Guid userId)
    {
        var license = await _productEntitlementManagementService.CreateEntitlementAsync(licenseId, productId, userId);

        return CreatedAtAction(nameof(CreateEntitlement), license);
    }

    /// <summary>
    /// Deletes product entitlement by ID.
    /// </summary>
    /// <param name="entitlementId">Product's entitlement unique identifier.</param>
    /// <returns>Deleted product entitlement entity.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// DELETE <code>license-management/license-api/entitlements?entitlementId=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpDelete]
    [SwaggerResponse(StatusCodes.Status200OK, "Returned successfully deleted item", typeof(ProductEntitlementDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> DeleteEntitlement([Required, FromQuery] Guid entitlementId)
    {
        var entitlement = await _productEntitlementManagementService.DeleteEntitlementAsync(entitlementId);

        return Ok(entitlement);
    }
}