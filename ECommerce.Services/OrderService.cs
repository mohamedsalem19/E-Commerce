using ECommerce.Core;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Order_Aggregate;
using ECommerce.Core.IRepositories;
using ECommerce.Core.IServices;
using ECommerce.Core.Specifications.OrderSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }


        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //get basket from baskets Repo
            var basket = await _basketRepository.GetBasketASync(basketId);

            //get selected item at basket from products repo
            var orderItems = new List<OrderItems>();

            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var productRepo = _unitOfWork.Repository<Product>();
                    if (productRepo is not null)
                    {
                        var product = await productRepo.GetByIdAsync(item.Id);
                        //if(product is null) return BadRequest(new ApiResponse(400));
                        var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                        var orderItem = new OrderItems(productItemOrdered, product.Price, item.Quantity);

                        orderItems.Add(orderItem);
                    }

                    
                }
            }

            

            //calculate subtotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            //get delivery method from delivery methods repo
            DeliveryMethod deliveryMethod = new DeliveryMethod();
            
            var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();  
           
            if(deliveryMethodRepo is not null)
             deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId);

            //create order
            var spec = new OrderWithPaymentIdWithSpecifcation(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (existingOrder is not null) 
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems,subtotal, basket.PaymentIntentId);

            var orderRepo = _unitOfWork.Repository<Order>();

            if (orderRepo is not null)
            {
                await orderRepo.Add(order);


                //save to database
                var result = await _unitOfWork.Complete();
                if (result > 0)
                    return order;

            }

            


            return null;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods; 
        }

        public  async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            
            if(order is null) return null;
            return order;
        
        }



        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            

            var spec = new OrderSpecification(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}
