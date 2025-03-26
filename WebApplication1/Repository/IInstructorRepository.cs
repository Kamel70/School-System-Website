using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IInstructorRepository
    {
        public List<Instructor> GetAll();
        public void Add(Instructor i);
        public void Update(Instructor i);
        public void delete(int id);
        public Instructor GetByID(int id);
        public void Save();
        public Instructor GetByIDIncludesCourses(int id);
        public Instructor GetByUserIDIncludesCourses(string id);
    }
}
