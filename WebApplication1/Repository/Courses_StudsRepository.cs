using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class Courses_StudsRepository:ICourses_StudsRepository
    {
        SchoolContext context;
        public Courses_StudsRepository(SchoolContext _school)
        {
            context = _school;
        }
        public List<Courses_Studs> GetAll()
        {
            return context.Courses_Studs.ToList();
        }
        public void Add(Courses_Studs cs)
        {
            context.Courses_Studs.Add(cs);
        }
        public void Update(Courses_Studs cs)
        {
            context.Courses_Studs.Update(cs);
        }
        public void delete(Courses_Studs cs)
        {
            context.Courses_Studs.Remove(cs);
        }
        public Courses_Studs GetByStudentID(int id)
        {
            return context.Courses_Studs.FirstOrDefault(cs => cs.StudentId == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }
        
    }
}
