namespace BlazorHRManagement.Api.Controllers;

public class UserController(IMediator mediator): HRManagementBaseController
{
    [HttpPost("[action]")]
    [ProducesDefaultResponseType(typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    => Ok(new ApiResponse<CreateUserVm>()
    {
        Success = true,
        Data = await mediator.SendCommandAsync<CreateUserCommand, CreateUserVm>(command, cancellationToken)
    });
}