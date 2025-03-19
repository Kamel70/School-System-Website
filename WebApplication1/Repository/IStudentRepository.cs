using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IStudentRepository
    {
        public List<Student> GetAll();
        public void Add(Student s);
        public void Update(Student s);
        public void delete(int id);
        public Student GetByID(int id);
        public void Save();
        public List<Student> getStudentsByDepartmentID(int id);

        public Student GetByIdIncludesCoursesAndCoursesStuds(int id);
    }
}
