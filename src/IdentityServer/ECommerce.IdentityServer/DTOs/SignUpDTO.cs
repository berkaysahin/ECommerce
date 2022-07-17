using System.ComponentModel.DataAnnotations;

namespace ECommerce.IdentityServer.DTOs
{
    public class SignUpDTO
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string City { get; set; }
    }
}
