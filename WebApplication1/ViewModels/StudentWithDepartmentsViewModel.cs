using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class StudentWithDepartmentsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
        public IFormFile Image { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
