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
    private readonly ISnsService _snsService;

    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="usersRepository"><see cref="IUsersRepository"/></param>
    /// <param name="logger">Logger instance.</param>
    public UsersController(IUsersRepository usersRepository, ILogger<UsersController> logger, ISnsService snsService)
    {
        _usersRepository = usersRepository;
        _logger = logger;

        _snsService = snsService;
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
    public void CreateUser([FromBody] string value)
    {
    }

    // PUT users
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody, Required] UserDto user)
    {
        try
        {
            await _usersRepository.SaveAsync(user);

            var userMessage = new BaseMessage<UserDto>(new Guid().ToString(), ProcessAction.Create);

            await _snsService.PublishToTopicAsync("arn:aws:sns:eu-central-1:812040966008:SampleTopic", userMessage);

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Saving user has failed.");
            return new JsonResult(new { ex.Message })
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