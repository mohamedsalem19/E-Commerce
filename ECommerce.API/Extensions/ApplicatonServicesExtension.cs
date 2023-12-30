using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Core;
using ECommerce.Core.IRepositories;
using ECommerce.Core.IServices;
using ECommerce.Repository;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Extensions
{
    public static class ApplicatonServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();


            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));

            services.AddScoped<IUnitOfWork,UnitOfWork>();


            services.AddAutoMapper(typeof(MappingProfiles));


            //Customize the response of validation Errors
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
             
            return services;    
        }
    }
}
