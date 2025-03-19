using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public ICollection<Courses_Studs> Courses_Studs { get; set; } = new HashSet<Courses_Studs>();
    }
}
