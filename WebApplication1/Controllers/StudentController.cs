using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {   
        IDepartmentRepository departmentRepository;
        IStudentRepository studentRepository;

        public StudentController(IDepartmentRepository departmentRepository, IStudentRepository studentRepository)
        {
            this.departmentRepository = departmentRepository;
            this.studentRepository = studentRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Departments=departmentRepository.GetAll();
            var students=studentRepository.GetAll();
            HttpContext.Session.SetString("UserName","Kamel");
            HttpContext.Session.SetInt32("Age", 24);
            return View(students);
        }

        public PartialViewResult GetStudentsByDepartment(int departmentId)
        {
            var students = departmentId == 0
                ? studentRepository.GetAll()
                : studentRepository.getStudentsByDepartmentID(departmentId);

            return PartialView("AllStudents", students);
        }

        public IActionResult Details(int id) 
        { 
            var student=studentRepository.GetByIdIncludesCoursesAndCoursesStuds(id);
            List<CourseViewModel> list=student.Courses_Studs.Select(sc=>new CourseViewModel
            {
                Name=sc.Courses.Name,
                minDegree = sc.Courses.minDigree,
                Degree=sc.degree
            })
                                                            .ToList();
            StudDetailsWithCoursesViewModel viewModel = new StudDetailsWithCoursesViewModel();
            viewModel.Name = student.Name;
            viewModel.Image=student.Image;
            viewModel.Address = student.Address;
            viewModel.Age = student.Age;
            viewModel.Courses = list;
            return View(viewModel);
        }

        public IActionResult ShowSessionData()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            StudentWithDepartmentsViewModel viewModel = new StudentWithDepartmentsViewModel();
            viewModel.Departments = departmentRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SaveAdd(StudentWithDepartmentsViewModel std)
        {
            if (std.Name != null && std.Address!=null)
            {
                //if (file == null || file.Length == 0)
                //    Console.WriteLine("file is null");

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
 

                //string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(std.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, std.Image.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    std.Image.CopyTo(fileStream);
                }

                Student newStud = new Student();
                newStud.Name = std.Name;
                newStud.Age = std.Age;
                newStud.Image = std.Image.FileName;
                newStud.Address = std.Address;
                newStud.DepartmentID = std.DepartmentID;
                studentRepository.Add(newStud);
                studentRepository.Save();
                return RedirectToAction("Index");
            }
            std.Departments = departmentRepository.GetAll();
            return View("Add",std);

        }
        public IActionResult Delete(int id) 
        {
            studentRepository.delete(id);
            studentRepository.Save();

            return RedirectToAction("Index");
        }

    }
}
