using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List <Department> listDepartments = new List<Department>();
            listDepartments.Add(new Department { Id = 1, Name = "Movies" });
            listDepartments.Add(new Department { Id = 2, Name = "TV Shows" });

            return View(listDepartments);
        }
    }
}
