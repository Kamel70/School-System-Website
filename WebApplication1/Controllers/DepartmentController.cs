using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    public class DepartmentController : Controller
    {
        public DepartmentRepository departmentRepository;
        public DepartmentController(DepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        public IActionResult Index()
        {
            var department=departmentRepository.GetAll();
            return View(department);
        }

        public IActionResult Details() { 
            return View();
        }
        [HttpGet]
        public IActionResult Add() 
        { 
            return View();
        }
        public IActionResult Add(Department dept)
        {
            return View();
        }
    }
}
