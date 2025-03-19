using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class StudentRepository:IStudentRepository
    {
        SchoolContext context;
        public StudentRepository(SchoolContext _school)
        {
            context = _school;
        }
        public List<Student> GetAll()
        {
            return context.Students.ToList();
        }
        public void Add(Student s)
        {
            context.Add(s);
        }
        public void Update(Student s)
        {
            context.Update(s);
        }
        public void delete(int id)
        {
            Student student = GetByID(id);
            context.Remove(student);
        }
        public Student GetByID(int id)
        {
            return context.Students.FirstOrDefault(s => s.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public List<Student> getStudentsByDepartmentID(int id)
        {
            return context.Students.Where(s => s.DepartmentID == id).ToList();
        }

        public Student GetByIdIncludesCoursesAndCoursesStuds(int id) 
        {
            return context.Students.Include(s => s.Courses_Studs)
                                   .ThenInclude(cs => cs.Courses)
                                   .FirstOrDefault(s => s.Id == id);
        }
    }
}
