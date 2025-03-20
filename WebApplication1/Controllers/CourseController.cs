    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    //[Authorize]
    public class CourseController : Controller
    {
        ICoursesRepository CoursesRepository;
        IInstructorRepository InstructorRepository;
        public CourseController(ICoursesRepository coursesRepository, IInstructorRepository instructorRepository)
        {
            CoursesRepository = coursesRepository;
            InstructorRepository = instructorRepository;
        }
        public IActionResult Index()
        {
            var courses=CoursesRepository.GetAll();
            return View(courses);
        }

        public IActionResult Details(int id)
        {
            var course= CoursesRepository.GetByIDIncludesInstructors(id);
            return View(course);
        }

        public IActionResult ShowSessionData()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            TempData["IsEdit"] = false;
            TempData.Keep("IsEdit");
            CourseWithInstructorsViewModel viewModel = new CourseWithInstructorsViewModel();
            viewModel.instructors = InstructorRepository.GetAll();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SaveAdd(CourseWithInstructorsViewModel course)
        {
            if (ModelState.IsValid)
            {
                Courses newCourse = new Courses();
                newCourse.Name = course.Name;
                newCourse.Topic = course.Topic;
                newCourse.minDigree=course.minDigree;
                newCourse.TotalDigree = course.TotalDigree;
                newCourse.InsID = course.InsID;
                CoursesRepository.Add(newCourse);
                CoursesRepository.Save();
                return RedirectToAction("Index");
            }
            course.instructors = InstructorRepository.GetAll();
            return View("Add", course);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            TempData["IsEdit"] = true;
            TempData.Keep("IsEdit");
            var course = CoursesRepository.GetByID(id);
            CourseWithInstructorsViewModel courseWithInstructors = new CourseWithInstructorsViewModel();
            courseWithInstructors.Id = course.Id;
            courseWithInstructors.Name = course.Name;
            courseWithInstructors.Topic = course.Topic;
            courseWithInstructors.minDigree = course.minDigree;
            courseWithInstructors.TotalDigree = course.TotalDigree;
            courseWithInstructors.InsID = course.InsID;
            courseWithInstructors.instructors = InstructorRepository.GetAll();
            return View(courseWithInstructors);
        }
        [HttpPost]
        public IActionResult Edit(CourseWithInstructorsViewModel courseRequest)
        {
            if (ModelState.IsValid)
            {
                Courses course = new Courses();
                course.Id = courseRequest.Id;
                course.Name = courseRequest.Name;
                course.Topic = courseRequest.Topic;
                course.minDigree = courseRequest.minDigree;
                course.TotalDigree = courseRequest.TotalDigree;
                course.InsID = courseRequest.InsID;
                CoursesRepository.Update(course);
                CoursesRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", courseRequest);
        }
        public IActionResult Delete(int id)
        {
            CoursesRepository.delete(id);
            CoursesRepository.Save();
            return RedirectToAction("Index");
        }
        public IActionResult uniqueName(string Name) 
        {
            Courses course = CoursesRepository.GetByName(Name);
            bool isEdit = TempData["IsEdit"] != null && (bool)TempData["IsEdit"];
            if (course != null && !isEdit) 
            {
                return Json(false);
            }
            return Json(true);
        }

        public IActionResult CheckTotalDegree(int TotalDigree, int minDigree) 
        {
            if (TotalDigree > minDigree)
            {
                return Json(true);
            }
            return Json(false);
        }
    }

    
}
