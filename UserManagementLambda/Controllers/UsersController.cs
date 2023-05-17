using Common.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using UserManagementLambda.Interfaces;

namespace UserManagementLambda.Controllers;

[Route("users")]
[ApiController]
[SwaggerTag("Controller used for users management")]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="usersRepository"><see cref="IUsersRepository"/></param>
    public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Hello world!");
    }

    // GET users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        _logger.LogInformation("Get method start.");

        UserDto user = await _usersRepository.GetByIdAsync(id);

        return Ok(user);
    }

    // POST users
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT users
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody, Required] UserDto user)
    {
        try
        {
            await _usersRepository.SaveAsync(user);

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Saving user has failed.");
            return new JsonResult(new { Message = ex.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    // DELETE users/{id}
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}