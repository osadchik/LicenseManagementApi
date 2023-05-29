using LicenseManagementLambda.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("{id}")]
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
    public async Task<IActionResult> CreateEntitlement([FromQuery, Required] Guid licenseId, [FromQuery, Required] Guid userId)
    {
        var license = await _productEntitlementManagementService.CreateEntitlementAsync(licenseId, userId);

        return CreatedAtAction(nameof(CreateEntitlement), license);
    }
}