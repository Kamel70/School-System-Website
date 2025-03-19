using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAll();
        public void Add(Department d);
        public void Update(Department d);
        public void delete(int id);
        public Department GetByID(int id);
        public void Save();

        public Department GetByIDIncludesInstructors(int id);
    }
}
