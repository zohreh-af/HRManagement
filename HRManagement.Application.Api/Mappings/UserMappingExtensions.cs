using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;
namespace BlazorHRManagement.Application.Api;

public static class UserMappingExtensions
{
    public static CreateUserCommand UserToCreateUserCommand(this User user)
    {
        return new CreateUserCommand
        {
            Id = user.Id,
            CreatorIdentityID = user.CreatorIdentityID,
            UserName = user.UserName,
            Email = user.Email,
            CreateDate = user.CreateDate,
            LastModifierIdentityID = user.LastModifierIdentityID,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            Name = user.Name,   
            Details = user.Details,
            Password = user.PasswordHash,   

        };
    }
    public static User CreateUserCommandToUser(this CreateUserCommand command)
    {
        return new User
        {
            Id = command.Id,
            CreatorIdentityID = command.CreatorIdentityID,
            UserName = command.UserName,
            Email = command.Email,
            CreateDate = command.CreateDate,
            LastModifierIdentityID = command.LastModifierIdentityID,
            EmailConfirmed = command.EmailConfirmed,
            PhoneNumberConfirmed = command.PhoneNumberConfirmed,
            PhoneNumber = command.PhoneNumber,
            IsActive = command.IsActive,
            Name = command.Name,
            Details = command.Details,
            PasswordHash = command.Password,

        };
    }

}