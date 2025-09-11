using Abstraction;
using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;
using HRManagement.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
namespace HRManagement.Application.Api.Features;

public class CreateUserHandler(
      IMapper mapper
    , ILogger<CreateUserHandler> logger
    , HRManagementContext dbContext
    , IPasswordHasher<User> passwordHasher) 
    : ICommandHandler<CreateUserCommand, CreateUserVm>
{
    public async Task<CreateUserVm> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            // 1) Basic validation
            if (command.Username == command.Password)
                return new CreateUserVm { Result = CreateUserResult.PasswordAndUsernameDuplicate };

            if (string.IsNullOrWhiteSpace(command.Password))
                return new CreateUserVm { Result = CreateUserResult.PasswordValidation };

            // 3) Duplicate check
            var exists = await dbContext.Users
                .AnyAsync(u => u.Username == command.Username, cancellationToken);

            if (exists)
                return new CreateUserVm { Result = CreateUserResult.DuplicateUsername };


            //  var newUser = mapper.Map<User>(command);
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = command.Username,
                IsActive = true,
                PersianName = "Zohreh Abbasifar",
            };

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, command.Password);

            dbContext.Users.Add(newUser);
            var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;

            logger.LogInformation($"");
            return new CreateUserVm
            {
                Result =CreateUserResult.Success
            };
        }
        catch (Exception ex )
        {
            logger.LogError(ex, "CreateUser failed for {Username}", command.Username);

            return new CreateUserVm { Result = CreateUserResult.FailedToRegister };
        }
    }
}