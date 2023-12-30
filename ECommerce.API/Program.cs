using ECommerce.API.Errors;
using ECommerce.API.Extensions;
using ECommerce.API.Helpers;
using ECommerce.API.MiddleWares;
using ECommerce.Core.Entities.Identity;
using ECommerce.Core.IRepositories;
using ECommerce.Core.IServices;
using ECommerce.Repository;
using ECommerce.Repository.Data;
using ECommerce.Repository.Identity;
using ECommerce.Repository.IdentityData;
using ECommerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                
                return  ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddApplicationServices();


            builder.Services.AddIdentityServices(builder.Configuration);

           
            
            #endregion

            var app = builder.Build();

            #region Execute Migration that is not applying

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();

                await dbContext.Database.MigrateAsync();

                await StoreContextSeed.seedAsync(dbContext); //data seed

                var identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

                await identityDbContext.Database.MigrateAsync();


                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await ApplicationIdentityDbContextSeeding.SeedUserAsync(userManager);
            
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error occured during apply the migration");
            }
            #endregion


            #region Configure Kestrel Middlewares

            app.UseMiddleware<ExceptionMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();    
            app.UseAuthorization();


            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}