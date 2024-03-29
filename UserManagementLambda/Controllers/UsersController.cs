using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Controllers;

/// <summary>
/// API controller for users management in License Management Service.
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
    [HttpGet("getById")]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully returned item", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> GetUserById([Required, FromQuery] Guid id)
    {
        UserDto user = await _userManagementService.GetUserByUuid(id);

        return Ok(user);
    }

    /// <summary>
    /// Get user with a specific username.
    /// </summary>
    /// <param name="username">User's unique identifier.</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpGet("getByUsername")]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully returned item", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> GetUserByUsername([Required, FromQuery] string username)
    {
        IList<UserDto> user = await _userManagementService.GetUserByUserName(username);

        return Ok(user);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/></param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully created item", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
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
    [SwaggerResponse(StatusCodes.Status201Created, "Returned successfully updated item", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
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
    [HttpDelete]
    [SwaggerResponse(StatusCodes.Status200OK, "Returned successfully deleted item", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incorrect input field value")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item does not present in the system")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unhandled exception occured")]
    public async Task<IActionResult> DeleteUser([FromQuery, Required] Guid id)
    {
        await _userManagementService.DeleteUser(id);

        return Accepted();
    }
}