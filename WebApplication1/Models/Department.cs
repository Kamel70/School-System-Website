using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2,ErrorMessage ="Name must be Between 2 to 10 characters")]
        [MaxLength(10, ErrorMessage = "Name must be Between 2 to 10 characters")]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Location { get; set; }
        public ICollection<Student>? Students { get; set; } = new HashSet<Student>();
        public ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();
    }
}
