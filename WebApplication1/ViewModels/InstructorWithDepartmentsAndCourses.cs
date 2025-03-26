using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class InstructorWithDepartmentsAndCourses
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The Password and Check Password must be the same")]
        public string CheckPassword { get; set; }
        public string phoneNumber { get; set; }
        public string Address { get; set; }

        [Display(Name="First Name")]
        [Required]
        [MinLength(3, ErrorMessage = "The First Name must be Greater Than 2 Character")]
        [MaxLength(10, ErrorMessage = "The First Name must be Less Than 10 Character")]
        [Remote(action:"CheckNameInDepartment",controller: "Instructor", AdditionalFields = "DeptID", ErrorMessage ="The Instructor Already exist in this department")]
        public string FName { get; set; }
        [Display(Name ="Last Name")]
        [Required]
        [MinLength(3, ErrorMessage = "The Last Name must be Greater Than 2 Character")]
        [MaxLength(10, ErrorMessage = "The Last Name must be Less Than 10 Character")]
        public string LName { get; set; }
        [Required]
        [Range(8000,100000,ErrorMessage ="You must match the range of Salaries from 8000 to 100000")]
        public int? Salary { get; set; }
        [Required]
        [Range(18,59,ErrorMessage = "minimum Age is 18 and maximum Age is 59")]
        public int? Age { get; set; }
        [Required]
        [RegularExpression(@"^.*\.(jpg|png)$", ErrorMessage = "Only .jpg and .png image files are allowed.")]
        public string? Image { get; set; }
        [Display(Name ="Hiring Date")]
        [DataType(DataType.Text)]
        [Required]
        public DateTime? HiringDate { get; set; }
        [Display(Name ="Department")]
        [Required]
        public int DeptID { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
        [Display(Name ="Course")]
        public int CourseId { get; set; }
        public List<Courses> courses { get; set; } = new List<Courses>();
    }
}
