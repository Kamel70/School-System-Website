using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    //[Route("ins")]
    [Authorize]
    public class InstructorController : Controller
    {
        IInstructorRepository instructorRepository;
        ICoursesRepository courseRepository;
        IDepartmentRepository departmentRepository;
        public InstructorController(IInstructorRepository instructorRepository,ICoursesRepository coursesRepository,IDepartmentRepository departmentRepository)
        {
            this.instructorRepository = instructorRepository;
            this.courseRepository = coursesRepository;
            this.departmentRepository = departmentRepository;
        }
        //[HttpGet("getall")]
        public IActionResult Index()
        {
            var instructor = instructorRepository.GetAll();
            return View(instructor);
        }
        //[HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var instructor = instructorRepository.GetByIDIncludesCourses(id);
            InstructorDetailsViewModel viewModel = new InstructorDetailsViewModel();
            viewModel.FullName = $"{instructor.FName} {instructor.LName}";
            viewModel.Image = instructor.Image;
            viewModel.HiringDate = instructor.HiringDate.ToString();
            List<Courses> courses = instructor.Courses.ToList();
            viewModel.Courses = courses;
            return View(viewModel);
        }
        //[HttpGet("add")]
        public IActionResult Add()
        {
            TempData["IsEdit"] = false; 
            TempData.Keep("IsEdit"); 
            InstructorWithDepartmentsAndCourses viewModel = new InstructorWithDepartmentsAndCourses();
            viewModel.courses = courseRepository.GetAll();
            viewModel.Departments = departmentRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SaveAdd(InstructorWithDepartmentsAndCourses ins)
        {
            if (ModelState.IsValid) 
            {
                Instructor newIns=new Instructor();
                Courses newCourses=new Courses();
                newIns.FName = ins.FName;
                newIns.LName = ins.LName;
                newIns.Salary = ins.Salary;
                newIns.Age = ins.Age;
                newIns.Image=ins.Image;
                newIns.HiringDate = ins.HiringDate;
                newIns.DeptID = ins.DeptID;
                newIns.Courses= courseRepository.getsByID(ins.CourseId);
                instructorRepository.Add(newIns);
                instructorRepository.Save();
                return RedirectToAction("Index");

            }
            ins.courses = courseRepository.GetAll();
            ins.Departments = departmentRepository.GetAll();
            return View("Add",ins);
            
        }

        //[HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            TempData["IsEdit"] = true;
            TempData.Keep("IsEdit");
            var ins=instructorRepository.GetByID(id);
            InstructorWithDepartmentsAndCourses viewModel = new InstructorWithDepartmentsAndCourses();
            viewModel.Id = id;
            viewModel.FName = ins.FName;
            viewModel.LName = ins.LName;
            viewModel.Salary = ins.Salary;
            viewModel.Age = ins.Age;
            viewModel.Image = ins.Image;
            viewModel.HiringDate = ins.HiringDate;
            viewModel.DeptID = ins.DeptID;
            viewModel.courses=courseRepository.GetAll();
            viewModel.Departments = departmentRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SaveEdit(InstructorWithDepartmentsAndCourses insRequest)
        {
            var ins=instructorRepository.GetByID(insRequest.Id);
            if (ModelState.IsValid)
            {
                ins.FName = insRequest.FName;
                ins.LName = insRequest.LName;
                ins.Salary = insRequest.Salary;
                ins.Age = insRequest.Age;
                ins.Image = insRequest.Image;
                ins.HiringDate = insRequest.HiringDate;
                ins.DeptID = insRequest.DeptID;
                ins.Courses = courseRepository.getsByID(insRequest.CourseId);
                instructorRepository.Update(ins);
                instructorRepository.Save();
                return RedirectToAction("Index");

            }
            insRequest.courses = courseRepository.GetAll();
            insRequest.Departments = departmentRepository.GetAll();
            return View("Edit",ins);
        }
        public IActionResult Delete(int id)
        {
            instructorRepository.delete(id);
            instructorRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult CheckNameInDepartment(string FName, int DeptID)
        {
            var department = departmentRepository.GetByIDIncludesInstructors(DeptID);
            bool exist = false;
            bool isEdit = TempData["IsEdit"] != null && (bool)TempData["IsEdit"];
            foreach (Instructor instructor in department.Instructors)
            {
                if (instructor.FName.ToString() == FName && !isEdit)
                {
                    exist = true;
                    break;
                }
            }

            if (exist)
            {
                return Json(false);
            }
            return Json(true);
        }
    }
}
