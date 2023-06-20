using Common.Entities;
using LicenseManagementLambda.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LicenseManagementLambda.Controllers;

/// <summary>
/// API controller for licenses management in License Management Service.
/// </summary>
[ApiController]
[Route("license-api/licenses")]
[Produces("application/json")]
public class LicenseController : ControllerBase
{
    private readonly ILicenseManagementService _licenseManagementService;
    private readonly ILogger<LicenseController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="LicenseController"/> class.
    /// </summary>
    /// <param name="licenseManagementService"><see cref="ILicenseManagementService"/></param>
    /// <param name="logger">Logger instance.</param>
    public LicenseController(ILicenseManagementService licenseManagementService, ILogger<LicenseController> logger)
    {
        _licenseManagementService = licenseManagementService;
        _logger = logger;
    }

    /// <summary>
    /// Gets license by ID.
    /// </summary>
    /// <param name="licenseId">License unique identifier.</param>
    /// <returns>License entity.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// GET <code>license-management/license-api/licenses?licenseId=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully returned item", typeof(LicenseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> GetLicense([Required, FromQuery] Guid licenseId)
    {
        var license = await _licenseManagementService.GetLicenseByIdAsync(licenseId);

        return Ok(license);
    }

    /// <summary>
    /// Creates new license.
    /// </summary>
    /// <param name="productId">Product unique identifier.</param>
    /// <param name="licenseModel"><see cref="LicenseCreateModel"/></param>
    /// <returns>Created license definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// POST <code>license-management/license-api/licences?productId=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully created item", typeof(LicenseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> CreateLicense([FromQuery, Required] Guid productId, [FromBody, Required]LicenseCreateModel licenseModel)
    {
        var license = await _licenseManagementService.CreateLicenseAsync(productId, licenseModel);

        return CreatedAtAction(nameof(CreateLicense), license);
    }

    /// <summary>
    /// Updates an existing license.
    /// </summary>
    /// <param name="licenseDto"><see cref="LicenseDto"/></param>
    /// <returns>Updated license definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// PUT <code>license-management/license-api/licences</code>
    /// </remarks>
    [HttpPut]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully updated item", typeof(LicenseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> UpdateLicense([FromBody, Required] LicenseDto licenseDto)
    {
        var license = await _licenseManagementService.UpdateLicenseAsync(licenseDto);

        return Ok(license);
    }

    /// <summary>
    /// Deletes an existing license.
    /// </summary>
    /// <param name="licenseId">License unique identifier.</param>
    /// <returns>Deleted license definition.</returns>
    /// <remarks>
    /// Example url call:
    /// 
    /// DELETE <code>license-management/license-api/licenses?licenseId=ebff8ad4-24f9-4be7-a15d-529f64ede7c6</code>
    /// </remarks>
    [HttpDelete]
    [SwaggerResponse(StatusCodes.Status200OK, "Returned successfully deleted item", typeof(LicenseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> DeleteLicense([Required, FromQuery] Guid licenseId)
    {
        var license = await _licenseManagementService.DeleteLicenseAsync(licenseId);

        return Ok(license);
    }
}