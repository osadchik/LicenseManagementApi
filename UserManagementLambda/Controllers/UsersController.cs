using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Controllers;

[Route("users")]
[ApiController]
[SwaggerTag("Controller for users management")]
public class UsersController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="usersRepository"><see cref="IUsersRepository"/></param>
    /// <param name="logger">Logger instance.</param>
    public UsersController(IUserManagementService userManagementService, ILogger<UsersController> logger)
    {
        _userManagementService = userManagementService;
        _logger = logger;
    }

    // GET users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        UserDto user = await _userManagementService.GetUserByUuid(id);

        return Ok(user);
    }

    // POST users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody, Required] UserDto user)
    {
        var createdUser = await _userManagementService.CreateUser(user);

        return Ok(createdUser);
    }

    // PUT users
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody, Required] UserDto user)
    {
        var updatedUser = await _userManagementService.UpdateUser(user);

        return Ok(updatedUser);
    }

    // DELETE users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedUser = await _userManagementService.DeleteUser(id);

        return Ok(deletedUser);
    }
}