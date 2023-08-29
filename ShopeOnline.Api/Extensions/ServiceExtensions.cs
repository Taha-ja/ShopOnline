using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopeOnline.Api.Data;

namespace ShopeOnline.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        public static void ConfigureSqlServerContext(this IServiceCollection services, IConfiguration config) {
            var connectionString = config["ConnectionStrings:ShopOnlineConnection"];
            services.AddDbContext<ShopOnlineDbContext>(o => o.UseSqlServer(connectionString));

        }
    }
}
