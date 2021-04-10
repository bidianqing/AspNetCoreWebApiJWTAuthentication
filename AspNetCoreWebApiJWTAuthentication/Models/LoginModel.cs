using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApiJWTAuthentication.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        // Test long to string
        public long? Id { get; set; }
    }
}
