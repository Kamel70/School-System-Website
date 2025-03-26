using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Models
{
    public class Courses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }   
        public int? minDigree {  get; set; }
        public int? TotalDigree { get; set; }
        public Instructor Instructor { get; set; }
        [ForeignKey("Instructor")]
        public int InsID { get; set; }
        public ICollection<Courses_Studs>Courses_Studs { get; set; } =new HashSet<Courses_Studs>(); 
    }
}
