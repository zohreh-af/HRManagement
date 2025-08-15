using HRManagement.Application.Abstraction;
using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;
using HRManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace HRManagement.Application.Api.Features;

public class CreateUserHandler(
      IMapper mapper
    , HRManagementContext dbContext
    , IPasswordHasher<User> passwordHasher) 
    : ICommandHandler<CreateUserCommand, CreateUserVm>
{
    public async Task<CreateUserVm> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // 1) Basic validation
        if (command.Username == command.Password)
            return new CreateUserVm { Result =CreateUserResult.PasswordAndUsernameDuplicate };

        if (string.IsNullOrWhiteSpace(command.Password))
            return new CreateUserVm { Result = CreateUserResult.PasswordValidation};

        // 3) Duplicate check
        var exists = await dbContext.Users
            .AnyAsync(u => u.Username == command.Username, cancellationToken);

        if (exists)
            return new CreateUserVm { Result = CreateUserResult.DuplicateUsername};


        //  var newUser = mapper.Map<User>(command);
        var newUser = new User
        {
            Username = command.Username,
            IsActive = true,
            PersianName = "Zohreh Abbasifar",
        };
        
        newUser.PasswordHash = passwordHasher.HashPassword(newUser, command.Password);

        dbContext.Users.Add(newUser);  
        var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

        return new CreateUserVm
        {
            Result = result ? CreateUserResult.Success : CreateUserResult.FailedToRegister
        };
    }

}