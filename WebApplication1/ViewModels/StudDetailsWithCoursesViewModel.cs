using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class StudDetailsWithCoursesViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        public List<CourseViewModel>Courses { get; set; }=new List<CourseViewModel>();
    }
}
