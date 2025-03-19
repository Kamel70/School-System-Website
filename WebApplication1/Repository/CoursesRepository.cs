using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CoursesRepository:ICoursesRepository
    {
        SchoolContext context;
        public CoursesRepository(SchoolContext _school)
        {
            context = _school;
        }
        public List<Courses> GetAll()
        {
            return context.Courses.ToList();
        }
        public void Add(Courses c)
        {
            context.Add(c);
        }
        public void Update(Courses c)
        {
            context.Update(c);
        }
        public void delete(int id)
        {
            Courses course= GetByID(id);
            context.Remove(course);
        }
        public Courses GetByID(int id)
        {
            return context.Courses.FirstOrDefault(c => c.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public Courses GetByIDIncludesInstructors(int id)
        {
            return context.Courses.Include(c => c.Instructor).FirstOrDefault(c => c.Id == id);
        }

        public Courses GetByName(string name)
        {
            return context.Courses.FirstOrDefault(c => c.Name == name);
        }

        public List<Courses> getsByID(int id)
        {
            return context.Courses.Where(c => c.Id == id).ToList();
        }
    }
}
