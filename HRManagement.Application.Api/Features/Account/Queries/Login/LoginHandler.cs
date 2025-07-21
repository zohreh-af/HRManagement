//using BlazorHRManagement.Application;
//using HRManagement.Application.Web.Features;
//using HRManagement.Domain.Entities;
//using HRManagement.Persistence.Contexts;

//namespace HRManagement.Application.Api.Features;

//public class LoginHandler(IMapper mapper, HRManagementContext dbContext) : IQueryHandler<LoginQuery, LoginVm>
//{
//    public async Task<LoginVm> HandleAsync(LoginQuery query, CancellationToken cancellationToken)
//    {
//        User user = await dbContext.Users.FirstOrDefault(u => u.UserName == query.UserName);

//        if (user == null) 
//        {
//            return new CreateUserVm
//            {
//                Result = false
//            };
//        }


//    }
//}
