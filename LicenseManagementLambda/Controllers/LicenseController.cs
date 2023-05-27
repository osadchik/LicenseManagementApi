using Common.Entities;
using LicenseManagementLambda.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LicenseManagementLambda.Controllers;

/// <summary>
/// Comtroller used to manipulate license entities in License Management Service.
/// </summary>
[Route("license-api/licenses")]
public class LicenseController : ControllerBase
{
    private readonly ILicenseManagementService _licenseManagementService;
    private readonly ILogger<LicenseController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="LicenseController"/> class.
    /// </summary>
    /// <param name="licenseManagementService"></param>
    /// <param name="logger"></param>
    public LicenseController(ILicenseManagementService licenseManagementService, ILogger<LicenseController> logger)
    {
        _licenseManagementService = licenseManagementService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLicense([Required]Guid licenseId)
    {
        var license = await _licenseManagementService.GetLicenseByIdAsync(licenseId);

        return Ok(license);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> CreateLicense([FromQuery, Required]Guid productId, [FromBody, Required]LicenseModel licenseModel)
    {
        var license = await _licenseManagementService.CreateLicenseAsync(productId, licenseModel);

        return CreatedAtAction(nameof(CreateLicense), license);
    }

    // PUT api/values/5
    [HttpPut]
    public async Task<IActionResult> UpdateLicense([FromBody, Required]LicenseDto licenseDto)
    {
        var license = await _licenseManagementService.UpdateLicenseAsync(licenseDto);

        return Ok(license);
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLicense([Required]Guid licenseId)
    {
        var license = await _licenseManagementService.DeleteLicenseAsync(licenseId);

        return Ok(license);
    }
}