using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class DepartmentRepository:IDepartmentRepository
    {
        SchoolContext context;
        public DepartmentRepository(SchoolContext _school)
        {
            context = _school;
        }
        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }
        public void Add(Department d)
        {
            context.Add(d);
        }
        public void Update(Department d)
        {
            context.Update(d);
        }
        public void delete(int id)
        {
            Department department = GetByID(id);
            context.Remove(department);
        }
        public Department GetByID(int id)
        {
            return context.Departments.FirstOrDefault(d => d.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public Department GetByIDIncludesInstructors(int id)
        {
            return context.Departments.Include(d => d.Instructors).FirstOrDefault(c => c.Id == id);
        }
    }
}
