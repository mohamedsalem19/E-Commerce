using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName  { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
            ErrorMessage ="password must have at least 1 uppercase, 1 lowercase , 1 number , 1 non alphanumeric and least 6 characters")]
        public string  Password { get; set; }

    }
}


