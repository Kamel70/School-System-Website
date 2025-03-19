using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class InstructorDetailsViewModel
    {
        public string FullName { get; set; }
        public string Image {  get; set; }

        public string HiringDate { get; set; }
        public List<Courses> Courses { get; set; }=new List<Courses>();
    }
}
