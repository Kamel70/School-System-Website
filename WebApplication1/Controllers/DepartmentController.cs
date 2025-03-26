using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        public IDepartmentRepository departmentRepository;
        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            departmentRepository = _departmentRepository;
        }

        public IActionResult Index()
        {
            var department=departmentRepository.GetAll();
            return View(department);
        }
        [HttpGet]
        public IActionResult Add() 
        { 
            return View();
        }
        [HttpPost]
        public IActionResult Add(Department dept)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Add(dept);
                departmentRepository.Save();
                return RedirectToAction("Index");
            }
                
            return View("Add",dept);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = departmentRepository.GetByID(id);
            return View(department);
        }
        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Update(dept);
                departmentRepository.Save();
                return RedirectToAction("Index");
            }

            return View("Edit", dept);
        }
        public IActionResult Details(int id)
        {
            var department = departmentRepository.GetByIDIncludesInstructors(id);
            return View(department);
        }
        
        public IActionResult Delete(int id)
        {
            departmentRepository.delete(id);
            departmentRepository.Save();
            return RedirectToAction("Index");
        }


    }
}
