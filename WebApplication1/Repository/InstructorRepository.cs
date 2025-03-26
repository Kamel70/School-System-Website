using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class InstructorRepository:IInstructorRepository
    {
        SchoolContext context;
        public InstructorRepository(SchoolContext _school)
        {
            context = _school;
        }
        public List<Instructor> GetAll()
        {
            return context.Instructors.ToList();
        }
        public void Add(Instructor i)
        {
            context.Instructors.Add(i);
        }
        public void Update(Instructor i)
        {
            context.Update(i);
        }
        public void delete(int id)
        {
            Instructor instructor = GetByID(id);
            context.Remove(instructor);
        }
        public Instructor GetByID(int id)
        {
            return context.Instructors.FirstOrDefault(i => i.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public Instructor GetByIDIncludesCourses(int id)
        {
            return context.Instructors.Include(i => i.Courses)
                                      .SingleOrDefault(i => i.Id == id);
        }

        public Instructor GetByUserIDIncludesCourses(string id)
        {
            return context.Instructors.Include(i => i.Courses)
                                     .SingleOrDefault(i => i.UserId == id);
        }
    }
}
