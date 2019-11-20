using BTJQ.Context;
using BTJQ.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BTJQ.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _employeeDbContext;
        public EmployeeController(EmployeeContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Gets()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault().ToLower();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var _employees = (from e in _employeeDbContext.tblEmployees
                                  join s in _employeeDbContext.tblSkills on e.SkillID equals s.SkillID
                                  select new EmployeeViewModel()
                                  {
                                      EmployeeID = e.EmployeeID,
                                      EmployeeName = e.EmployeeName,
                                      PhoneNumber = e.PhoneNumber,
                                      Skill = s.Title,
                                      YearsExperience = e.YearsExperience
                                  }).ToList();

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    var prop = GetProperty(sortColumn);
                    if (sortColumnDirection == "asc")
                    {
                        _employees = _employees.OrderBy(prop.GetValue).ToList();
                    }
                    else
                    {
                        _employees = _employees.OrderByDescending(prop.GetValue).ToList();
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    _employees = (from e in _employees
                                  where e.EmployeeName.ToLower().Contains(searchValue) ||
                                       e.Skill.ToLower().Contains(searchValue) ||
                                       e.PhoneNumber.ToLower().Contains(searchValue)
                                  select e).ToList();
                }

                //total number of rows count   
                recordsTotal = _employees.Count();
                //Paging   
                var data = _employees.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }

        private PropertyInfo GetProperty(string name)
        {
            var properties = typeof(EmployeeViewModel).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    prop = item;
                    break;
                }
            }
            return prop;
        }

        public ActionResult GetSkill()
        {
            var skills = new List<SkillModel>();
            skills = _employeeDbContext.tblSkills.Select(s => new SkillModel()
            {
                Name = s.Title,
                Id = s.SkillID
            }).ToList();

            return Json(new { response = skills, code = 1 });
        }

        [HttpPost]
        public IActionResult Save([FromBody] EmployeeCreateModel model)
        {

            var employee = new tblEmployee()
            {
                EmployeeID = model.EmployeeID,
                EmployeeName = model.EmployeeName,
                PhoneNumber = model.PhoneNumber,
                SkillID = model.SkillID,
                YearsExperience = model.YearsExperience
            };
            if (model.EmployeeID == 0)
            {
                _employeeDbContext.tblEmployees.Add(employee);
            }
            else
            {
                _employeeDbContext.tblEmployees.Update(employee);
            }

            if (_employeeDbContext.SaveChanges() > 0)
            {
                TempData["Message"] = "Employee has been added successfully.";
            }
            else
            {
                TempData["Message"] = "Something went wrong, please contact administrator.";
            }
            ViewBag.Skills = GetSkill();
            return new JsonResult(new { status = 1 });
            //return RedirectToAction(nameof(Index));
            //return View(new EmployeeCreateModel());
        }

        public JsonResult Get(int id)
        {
            var employee = _employeeDbContext.tblEmployees.Find(id);

            if (employee == null)
            {
                return new JsonResult(new { status = -1 });
            }


            return Json(new { response = employee, code = 1 });
            //return new JsonResult(new { status = -1 });
        }
    }
}