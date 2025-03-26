using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        UserManager<ApplicationUser> userManager;   
        public InstructorController(
            IInstructorRepository instructorRepository
            ,ICoursesRepository coursesRepository
            ,IDepartmentRepository departmentRepository
            ,UserManager<ApplicationUser> _userManager)
        {
            this.instructorRepository = instructorRepository;
            this.courseRepository = coursesRepository;
            this.departmentRepository = departmentRepository;
            userManager = _userManager;
        }
        //[HttpGet("getall")]
        [Authorize(Roles = "HR,Admin")]
        public IActionResult Index()
        {
            var instructor = instructorRepository.GetAll();
            return View(instructor);
        }
        //[HttpGet("{id}")]
        [Authorize(Roles = "HR")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveAdd(InstructorWithDepartmentsAndCourses ins)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = ins.Email;
                user.PasswordHash = ins.Password;
                user.UserName = $"{ins.FName}-{ins.LName}";
                user.PhoneNumber = ins.phoneNumber;
                user.Address = ins.Address;
                IdentityResult id = await userManager.CreateAsync(user);
                if(id.Succeeded)
                {
                    IdentityResult idRole=await userManager.AddToRoleAsync(user, "Instructor");
                    if (idRole.Succeeded)
                    {
                        Instructor newIns = new Instructor();
                        Courses newCourses = new Courses();
                        newIns.FName = ins.FName;
                        newIns.LName = ins.LName;
                        newIns.Salary = ins.Salary;
                        newIns.Age = ins.Age;
                        newIns.Image = ins.Image;
                        newIns.HiringDate = ins.HiringDate;
                        newIns.UserId=user.Id;
                        newIns.DeptID = ins.DeptID;
                        newIns.Courses = courseRepository.getsByID(ins.CourseId);
                        instructorRepository.Add(newIns);
                        instructorRepository.Save();
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            ins.courses = courseRepository.GetAll();
            ins.Departments = departmentRepository.GetAll();
            return View("Add",ins);
            
        }

        //[HttpGet("edit/{id}")]
        [Authorize(Roles = "HR")]
        public IActionResult Edit(int id)
        {
            TempData["IsEdit"] = true;
            TempData.Keep("IsEdit");
            var ins=instructorRepository.GetByID(id);
            if (ins != null) 
            {
                InstructorWithDepartmentsAndCourses viewModel = new InstructorWithDepartmentsAndCourses();
                viewModel.Id = id;
                viewModel.FName = ins.FName;
                viewModel.LName = ins.LName;
                viewModel.Salary = ins.Salary;
                viewModel.Age = ins.Age;
                viewModel.Image = ins.Image;
                viewModel.HiringDate = ins.HiringDate;
                viewModel.DeptID = ins.DeptID;
                ApplicationUser user=userManager.Users.FirstOrDefault(u=>u.Id==ins.UserId);
                viewModel.Email = user.Email;
                viewModel.Password = user.PasswordHash;
                viewModel.Address = user.Address;
                viewModel.phoneNumber = user.PhoneNumber;   
                viewModel.courses = courseRepository.GetAll();
                viewModel.Departments = departmentRepository.GetAll();
                return View(viewModel);
            }
            return NotFound(); 
        }
        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> SaveEdit(InstructorWithDepartmentsAndCourses insRequest)
        {
            var ins=instructorRepository.GetByID(insRequest.Id);
            var user = userManager.Users.FirstOrDefault(u => u.Id == ins.UserId);
            if (ModelState.IsValid)
            {
                user.Email = insRequest.Email;
                user.PhoneNumber = insRequest.phoneNumber;
                user.PasswordHash = insRequest.Password;
                user.Address = insRequest.Address;
                await userManager.UpdateAsync(user);
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
        [Authorize(Roles = "HR")]
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
