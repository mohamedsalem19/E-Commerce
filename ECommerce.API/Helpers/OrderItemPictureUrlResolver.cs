using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.Core.Entities.Order_Aggregate;

namespace ECommerce.API.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItems, OrderItemsDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItems source, OrderItemsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;
        }
    }
}
