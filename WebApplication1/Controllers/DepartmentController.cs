﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
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

        public IActionResult Details() { 
            return View();
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
    }
}
