using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Controllers;

/// <summary>
/// Comtroller used to manipulate user entities in License Management Service.
/// </summary>
[ApiController]
[Route("users-api/users")]
[SwaggerTag("Controller for users management")]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>   
    /// Initializes a new instance of <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userManagementService"><see cref="IUserManagementService"/></param>
    /// <param name="logger">Logger instance.</param>
    public UsersController(IUserManagementService userManagementService, ILogger<UsersController> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    /// <summary>
    /// Get user with a specific Uuid.
    /// </summary>
    /// <param name="id">User's unique identifier.</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        UserDto user = await _userManagementService.GetUserByUuid(id);

        return Ok(user);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/></param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody, Required] UserDto user)
    {
        var createdUser = await _userManagementService.CreateUser(user);

        return Accepted(createdUser);
    }

    /// <summary>
    /// Updates a specific user.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/></param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody, Required] UserDto user)
    {
        var updatedUser = await _userManagementService.UpdateUser(user);

        return Accepted(updatedUser);
    }

    /// <summary>
    /// Deletes a user with specific Uuid.
    /// </summary>
    /// <param name="id">User's unique indentifier.</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _userManagementService.DeleteUser(id);

        return Accepted();
    }
}