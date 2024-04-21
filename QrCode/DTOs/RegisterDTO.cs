using System.ComponentModel.DataAnnotations;

namespace QrCode.API.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }   
        public string Password { get; set; }    }
}
