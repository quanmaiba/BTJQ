using BTJQ.Context;
using BTJQ.Models;
using BTJQ.ModelView;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BTJQ.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeContext _dbContext;

        public HomeController(EmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Gets()
        {
            try
            {
                var students = new List<StudentModelView>();
                students = (from s in _dbContext.Students
                            join c in _dbContext.ClassRooms on s.ClassRoomId
                            equals c.ClassRoomId
                            select new StudentModelView()
                            {
                                ClassName = c.ClassName,
                                DOB = s.DOB.ToString("dd/MM/yyyy"),
                                FullName = s.FullName,
                                Sex = s.Sex ? "Nam" : "Nữ",
                                StudentId = s.StudentId
                            }).ToList();

                return new JsonResult(new { response = students, status = 1 });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = -1, message = "Lỗi" });
            }

        }


        public IActionResult Index()
        {
            var _employees = _dbContext.tblEmployees.
                            Join(_dbContext.tblSkills, e => e.SkillID, s => s.SkillID,
                            (e, s) => new EmployeeViewModel
                            {
                                EmployeeID = e.EmployeeID,
                                EmployeeName = e.EmployeeName,
                                PhoneNumber = e.PhoneNumber,
                                Skill = s.Title,
                                YearsExperience = e.YearsExperience
                            }).ToList();
            IList<EmployeeViewModel> employees = _employees;
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Skills = GetSkills();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateModel model)
        {
            var employee = new tblEmployee()
            {
                EmployeeID = model.EmployeeID,
                EmployeeName = model.EmployeeName,
                PhoneNumber = model.PhoneNumber,
                SkillID = model.SkillID,
                YearsExperience = model.YearsExperience
            };
            _dbContext.tblEmployees.Add(employee);
            if (_dbContext.SaveChanges() > 0)
            {
                TempData["Message"] = "Employee has been added successfully.";
            }
            else
            {
                TempData["Message"] = "Something went wrong, please contact administrator.";
            }
            ViewBag.Skills = GetSkills();
            return View(new EmployeeCreateModel());
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _dbContext.tblEmployees.Find(id);
            _dbContext.Remove(employee);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var _employees = _dbContext.tblEmployees.
                            Join(_dbContext.tblSkills, e => e.SkillID, s => s.SkillID,
                            (e, s) => new EmployeeViewModel
                            {
                                EmployeeID = e.EmployeeID,
                                EmployeeName = e.EmployeeName,
                                PhoneNumber = e.PhoneNumber,
                                Skill = s.Title,
                                YearsExperience = e.YearsExperience
                            }).Where(e => e.EmployeeID == id).FirstOrDefault();
            return View(_employees);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var _employees = _dbContext.tblEmployees.Where(e => e.EmployeeID == id).FirstOrDefault();
            var employeeEdit = new EmployeeEditModel()
            {
                EmployeeID = _employees.EmployeeID,
                EmployeeName = _employees.EmployeeName,
                PhoneNumber = _employees.PhoneNumber,
                SkillID = _employees.SkillID,
                YearsExperience = _employees.YearsExperience
            };
            ViewBag.Skills = GetSkills();
            return View(employeeEdit);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditModel model)
        {
            var employee = _dbContext.tblEmployees.Find(model.EmployeeID);
            employee.EmployeeName = model.EmployeeName;
            employee.PhoneNumber = model.PhoneNumber;
            employee.SkillID = model.SkillID;
            employee.YearsExperience = model.YearsExperience;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<SkillModel> GetSkills()
        {
            var skills = _dbContext.tblSkills.Select(s => new SkillModel()
            {
                Name = s.Title,
                Id = s.SkillID
            }).ToList();
            return skills;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult Gets()
        //{
        //    var employees = new List<Employee>()
        //    {
        //        new Employee(){Id = 1 , EmployeeName="Anh Quan", Sex = true, BOB=DateTime.Parse("1998/01/01"),Position="Deverloper"},
        //        new Employee(){Id = 2 , EmployeeName="Anh Quan 1", Sex = true, BOB=DateTime.Parse("1998/01/01"),Position="Deverloper"},
        //        new Employee(){Id = 3 , EmployeeName="Anh Quan 2", Sex = false, BOB=DateTime.Parse("1998/01/01"),Position="Deverloper"},
        //    };
        //    return new JsonResult(new { response = employees,status = 1});
        //}



        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }

    //public class Employee
    //{
    //    public int Id { get; set; }
    //    public string EmployeeName { get; set; }
    //    public bool Sex { get; set; }
    //    public DateTime BOB { get; set; }
    //    public string Position { get; set; }
    //}



}
