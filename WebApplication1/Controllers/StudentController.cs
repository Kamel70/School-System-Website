﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    //[Authorize(Roles ="Admin,HR,Student")]
    public class StudentController : Controller
    {
        IDepartmentRepository departmentRepository;
        IStudentRepository studentRepository;
        ICoursesRepository coursesRepository;
        ICourses_StudsRepository courses_StudsRepository;
        UserManager<ApplicationUser> userManager;

        public StudentController(IDepartmentRepository departmentRepository
            , IStudentRepository studentRepository
            , UserManager<ApplicationUser> userManager
            , ICoursesRepository coursesRepository,
              ICourses_StudsRepository courses_StudsRepository
            )
        {
            this.departmentRepository = departmentRepository;
            this.studentRepository = studentRepository;
            this.userManager = userManager;
            this.coursesRepository = coursesRepository;
            this.courses_StudsRepository = courses_StudsRepository;
        }
        [Authorize(Roles = "HR,Admin")]
        public IActionResult Index()
        {
            ViewBag.Departments = departmentRepository.GetAll();
            var students = studentRepository.GetAll();
            HttpContext.Session.SetString("UserName", "Kamel");
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
        [Authorize(Roles = "HR")]
        public IActionResult Details(int id)
        {
            var student = studentRepository.GetByIdIncludesCoursesAndCoursesStuds(id);
            List<CourseViewModel> list = student.Courses_Studs.Select(sc => new CourseViewModel
            {
                Name = sc.Courses.Name,
                minDegree = sc.Courses.minDigree,
                Degree = sc.degree
            })
                                                            .ToList();
            StudDetailsWithCoursesViewModel viewModel = new StudDetailsWithCoursesViewModel();
            viewModel.Name = student.Name;
            viewModel.Image = student.Image;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            StudentWithDepartmentsViewModel viewModel = new StudentWithDepartmentsViewModel();
            viewModel.Departments = departmentRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveAdd(StudentWithDepartmentsViewModel std)
        {
            if (std.Name != null && std.Address != null)
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
                ApplicationUser user = new ApplicationUser();
                user.Email = std.Email;
                user.PasswordHash = std.Password;
                user.UserName = std.Name;
                user.PhoneNumber = std.PhoneNumber;
                user.Address = std.Address;
                IdentityResult id = await userManager.CreateAsync(user);
                if (id.Succeeded)
                {
                    IdentityResult idRole = await userManager.AddToRoleAsync(user, "Student");
                    if (idRole.Succeeded)
                    {
                        Student newStud = new Student();
                        newStud.Name = std.Name;
                        newStud.Age = std.Age;
                        newStud.Image = std.Image.FileName;
                        newStud.Address = std.Address;
                        newStud.UserId = user.Id;
                        newStud.DepartmentID = std.DepartmentID;
                        studentRepository.Add(newStud);
                        studentRepository.Save();
                        return RedirectToAction("Index");
                    }
                }
            }
            std.Departments = departmentRepository.GetAll();
            return View("Add", std);

        }
        [HttpGet]
        [Authorize(Roles = "HR")]
        public IActionResult Edit(int id)
        {
            Student std = studentRepository.GetByID(id);
            ApplicationUser user= userManager.Users.FirstOrDefault(u => u.Id == std.UserId);
            StudentWithDepartmentsViewModel viewModel = new StudentWithDepartmentsViewModel();
            viewModel.Id = std.Id;
            viewModel.Name = std.Name;
            viewModel.Age = std.Age;
            viewModel.Address = std.Address;
            viewModel.Email = user.Email;
            viewModel.PhoneNumber = user.PhoneNumber;
            viewModel.Password = user.PasswordHash;
            viewModel.DepartmentID = std.DepartmentID;
            viewModel.Departments = departmentRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Edit(StudentWithDepartmentsViewModel std)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Id = studentRepository.GetByID(std.Id).UserId;
                user.Email = std.Email;
                user.PasswordHash = std.Password;
                user.UserName = std.Name;
                user.PhoneNumber = std.PhoneNumber;
                user.Address = std.Address;
                await userManager.UpdateAsync(user);
                Student newStud = new Student();
                newStud.Id = std.Id;
                newStud.Name = std.Name;
                newStud.Age = std.Age;
                newStud.Image = std.Image.FileName;
                newStud.Address = std.Address;
                newStud.DepartmentID = std.DepartmentID;
                studentRepository.Update(newStud);
                studentRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit",std);
        }
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Delete(int id) 
        {
            studentRepository.delete(id);
            studentRepository.Save();
            ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Id == studentRepository.GetByID(id).UserId);
            await userManager.DeleteAsync(user);
            await userManager.RemoveFromRoleAsync(user, "Student");

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles ="Student")]
        public IActionResult GetDetailsOfStd()
        {
            Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string id = claim.Value;
            var student = studentRepository.GetByUserIdIncludesCoursesAndCoursesStuds(id);
            List<CourseViewModel> list = student.Courses_Studs.Select(sc => new CourseViewModel
            {
                Name = sc.Courses.Name,
                minDegree = sc.Courses.minDigree,
                Degree = sc.degree
            })
                                                            .ToList();
            StudDetailsWithCoursesViewModel viewModel = new StudDetailsWithCoursesViewModel();
            viewModel.Name = student.Name;
            viewModel.Image = student.Image;
            viewModel.Address = student.Address;
            viewModel.Age = student.Age;
            viewModel.Courses = list;
            return View("Details", viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult ShowCourses()
        {
            Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string id = claim.Value;
            var student = studentRepository.GetByUserIdIncludesCoursesAndCoursesStuds(id);
            List<CourseViewModel> list = student.Courses_Studs.Select(sc=> new CourseViewModel
            {
                Name = sc.Courses.Name,
                minDegree = sc.Courses.minDigree,
                Degree = sc.degree
            })
                                                               .ToList() ;
            return View(list);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult AddCourse()
        {
            List<Courses> courses =coursesRepository.GetAll();
            return View(courses);
        }
        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult AddCourse(int cID)
        {
            Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            string id = claim.Value;
            var student = studentRepository.GetByUserIdIncludesCoursesAndCoursesStuds(id);
            Courses_Studs courses_Studs=new Courses_Studs();
            courses_Studs.StudentId = student.Id;
            courses_Studs.CoursesId = cID;
            courses_StudsRepository.Add(courses_Studs);
            courses_StudsRepository.Save();
            return RedirectToAction("ShowCourses");
        }

    }
}
