

using BlazorHRManagement.Application.Api;
using HRManagement.Application.Api.Abstraction;
using HRManagement.Application.Web.Features;
using HRManagement.Persistence.Contexts;
namespace HRManagement.Application.Api.Features;

public class CreateUserHandler : ICommandHandler<CreateUserCommand,CreateUserVm>
{
    private readonly HRManagementContext _dbContext;

    public CreateUserHandler(HRManagementContext dbContext) => _dbContext = dbContext;

    public async Task<CreateUserVm> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = command.CreateUserCommandToUser();

        _dbContext.Users.Add(newUser);  // add user
        var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0; // save to DB

        return new CreateUserVm
        {
            Result = result
        };
    }

}