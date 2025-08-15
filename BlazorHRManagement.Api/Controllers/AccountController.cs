
using Abstraction;

namespace BlazorHRManagement.Api.Controllers;

public class AccountController(IMediator mediator) : HRManagementBaseController
{
    [HttpPost("[action]")]
    [ProducesDefaultResponseType(typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    => Ok(new ApiResponse<CreateUserVm>()
    {
        Successful = true,
        Data = await mediator.SendCommandAsync<CreateUserCommand, CreateUserVm>(command, cancellationToken)
    });

    [HttpPost("[action]")]
    [ProducesDefaultResponseType(typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserQuery query, CancellationToken cancellationToken)
    => Ok(new ApiResponse<LoginUserVm>()
    {
        Successful = true,
        Data = await mediator.SendQueryAsync<LoginUserQuery, LoginUserVm>(query, cancellationToken)
    });

    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {

        return Ok("successful");
    }
}