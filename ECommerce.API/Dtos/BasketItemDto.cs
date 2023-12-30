using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.1, double.MaxValue)]

        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "Quantity must be one item at least")]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Category { get; set; }

    }
}