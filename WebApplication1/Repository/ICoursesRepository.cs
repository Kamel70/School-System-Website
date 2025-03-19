using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface ICoursesRepository
    {
        public List<Courses> GetAll();
        public void Add(Courses c);
        public void Update(Courses c);
        public void delete(int id);
        public Courses GetByID(int id);
        public void Save();

        public Courses GetByIDIncludesInstructors(int id);

        public Courses GetByName(string name);

        public List<Courses> getsByID(int id);
    }
}
