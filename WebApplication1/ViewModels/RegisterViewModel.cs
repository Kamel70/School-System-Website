using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(2,ErrorMessage ="The Name Must be Between 2 to 20 Character")]
        [MaxLength(20,ErrorMessage = "The Name Must be Between 2 to 20 Character")]
        [Display(Name ="User Name")]
        public string FName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",ErrorMessage = "Password must be Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; } 
        [Required]
        [Compare("Password",ErrorMessage ="Confirm Password Must Match Password")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirmed Password")]
        public string ConfirmedPassowrd { get; set; }
        [Required]
        [RegularExpression("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$",ErrorMessage ="Enter a Valid Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string SelectedRole { get; set; }
        
        public List<SelectListItem>? Roles { get; set; }



    }
}
