using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class Account
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
