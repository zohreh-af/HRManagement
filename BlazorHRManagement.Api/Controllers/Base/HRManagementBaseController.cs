using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace BlazorHRManagement.Api.Controllers;

[ApiController]
[EnableCors("OpenCors")]
[Route("[controller]")]

public class HRManagementBaseController : ControllerBase
{

}
