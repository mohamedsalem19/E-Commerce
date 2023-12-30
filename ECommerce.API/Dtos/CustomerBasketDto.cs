using ECommerce.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        public List<BasketItemDto> Items { get; set; }
    }
}
