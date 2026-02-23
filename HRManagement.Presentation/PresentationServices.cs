using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Presentation;

public static class PresentationServices
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<AppTitleState>();
    }
}