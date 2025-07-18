using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHRManagement.Api.Controllers;

[ApiController]
[EnableCors("OpenCors")]
[Route("HR/[controller]")]

public class HRManagementBaseController : ControllerBase
{

}
