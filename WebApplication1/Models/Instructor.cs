using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int? Salary { get; set; }
        public int? Age { get; set; }
        public string? Image {  get; set; }
        public DateTime? HiringDate { get; set; }

        public Department Department { get; set; }
        [ForeignKey("Department")]
        public int DeptID { get; set; }
        public ICollection<Courses> Courses { get; set; } = new HashSet<Courses>();
    }
}
