using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace BlazorHRManagement.Api.Controllers;

[ApiController]
[Authorize]
[EnableCors("OpenCors")]
[Route("HR/[controller]")]

public class HRManagementBaseController : ControllerBase
{

}
