using HRManagement.Application.Web.Features;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Api.Features;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<CreateUserCommand, User>();
    }
}