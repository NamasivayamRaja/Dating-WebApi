using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration configuration) 
        {
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors();

            services.AddScoped<ITokenService, TokenService>();

            return services;

        }
    }
}
