using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Courses_Studs
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Courses Courses { get; set; }
        [ForeignKey("Courses")]
        public int CoursesId { get;set; }
        public int degree { get; set; }
    }
}
