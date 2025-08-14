using HRManagement.Application.Api.Abstraction;
using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;
using HRManagement.Persistence.Contexts;
namespace HRManagement.Application.Api.Features;

public class CreateUserHandler(IMapper mapper, HRManagementContext dbContext) : ICommandHandler<CreateUserCommand, CreateUserVm>
{
    public async Task<CreateUserVm> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = mapper.Map<User>(command); // map command to User entity  

        dbContext.Users.Add(newUser);  // add user
        var result = await dbContext.SaveChangesAsync(cancellationToken) > 0; // save to DB

        return new CreateUserVm
        {
            Result = result
        };
    }

}