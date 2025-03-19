using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface ICourses_StudsRepository
    {
        public List<Courses_Studs> GetAll();
        public void Add(Courses_Studs cs);
        public void Update(Courses_Studs cs);
        public void delete(Courses_Studs cs);
        public Courses_Studs GetByStudentID(int id);

        public void Save();
    }
}
