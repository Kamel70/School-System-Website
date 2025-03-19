using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public ICollection<Instructor> Instructors { get; set; } = new HashSet<Instructor>();
    }
}
