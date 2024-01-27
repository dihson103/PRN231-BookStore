using App_API.MiddleWares;
using App_API.Models;
using App_API.Repositories;
using App_API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace App_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<Assignment2Prn231Context>
                    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddScoped<IPublisherService, PublisherService>();
            builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:7243")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleWare>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseCors("AllowSpecificOrigin");

            app.Run();
        }
    }
}