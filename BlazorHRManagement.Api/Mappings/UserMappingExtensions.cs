using HRManagement.Domain.Entities;
using BlazorHRManagement.Api.Dtos;

namespace BlazorHRManagement.Api;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
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

}