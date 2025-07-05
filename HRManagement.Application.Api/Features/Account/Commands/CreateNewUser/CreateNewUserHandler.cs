using BlazorHRManagement.Api.Abstraction;
using BlazorHRManagement.Application.Web.Features;
using HRManagement.Persistence.Contexts;

namespace HRManagement.Application.Api.Features;

public class CreateNewUserHandler: ICommandHandler<CreateUserCommand, CreateUserVm>
{
    private readonly HRManagementContext _db;

    public CreateNewUserHandler(HRManagementContext db) => _db = db;

    public async Task<CreateUserCommand> HandleAsync (CreateUserCommand command, CancellationToken)
    {
        var newUser = 
    }

}