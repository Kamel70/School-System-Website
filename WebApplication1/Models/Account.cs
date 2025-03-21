using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class Account
    {
        [Required]
        [Display(Name="User Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
