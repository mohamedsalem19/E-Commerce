using ECommerce.Core;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.IRepositories;
using ECommerce.Core.IServices;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = ECommerce.Core.Entities.Product;

namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration ,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var basket = await _basketRepository.GetBasketASync(basketId);
           
            if (basket is null) return null;

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue) 
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                basket.ShippingCost = deliveryMethod.Cost;
                
                shippingPrice = deliveryMethod.Cost; 
            }

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items) 
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }

            }

            PaymentIntent paymentIntent;

            var service  = new PaymentIntentService();  

            if(string.IsNullOrEmpty(basket.PaymentIntentId)) // Create Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) basket.Items.Sum(item => item.Price * item.Quantity *100) + (long) shippingPrice *100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}

                };

                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;            }
            else  // Update Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,

                };

                await service.UpdateAsync(basket.PaymentIntentId,options);

            }
            
            await _basketRepository.UpdateBasketAsync(basket);

            return (basket);

        }
    }
}
