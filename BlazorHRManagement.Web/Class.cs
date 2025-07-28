using HRManagement.Persistence;

namespace BlazorHRManagement.Web;

    public class Test
    {
        public void Try(IServiceCollection services, IConfiguration config)
        {
            services.AddPersistenceServices(config); // باید بدون ارور باشه
        }
    }


