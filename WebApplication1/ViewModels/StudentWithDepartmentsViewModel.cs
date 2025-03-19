using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class StudentWithDepartmentsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public IFormFile Image { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}
