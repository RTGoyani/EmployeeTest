using EmployeeTest.DataContext;
using EmployeeTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTest.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmpContext empContext;
        public EmployeeController(EmpContext empContext)
        {
            this.empContext = empContext;
        }
        [HttpGet]
        public async Task<IActionResult> Add(int id = 0)
        {
            if (id > 0)
            {
                var employee = await empContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
                if (employee != null)
                {
                    var viewModel = new Employee()
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        City = employee.City,
                        Zip = employee.Zip,
                        CreatedDate = employee.CreatedDate
                    };
                    return View("Add", viewModel);
                }
                else
                {
                    return RedirectToAction("Add");
                }
            }
            else
            {
                var viewModel = new Employee();
                return View("Add", viewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await empContext.Employees.ToListAsync();
            return View(employees);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubmit(Employee model)
        {
            if (model.Id > 0)
            {
                var employee = await empContext.Employees.FirstOrDefaultAsync(x => x.Id == model.Id);

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.City = model.City;
                employee.Zip = model.Zip;
                employee.CreatedDate = System.DateTime.Now;

                await empContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                var employes = new Employee()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City,
                    Zip = model.Zip,
                    CreatedDate = System.DateTime.Now
                };
                await empContext.Employees.AddAsync(employes);
                await empContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        

        }
        [HttpGet]
        public async Task<IActionResult> EmpSalary(int Employeeid = 0)
        {
            EmployeeSalary empsal = new EmployeeSalary();
            empsal.EmployeeId = Employeeid;
            empsal.SalaryDate = DateTime.Today;
            return View("EmpSalary", empsal);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitSalary(EmployeeSalary model)
        {
            var empsalary = new EmployeeSalary()
            {
                EmployeeId = model.EmployeeId,
                SalaryDate = model.SalaryDate,
                Amount = model.Amount,
                CreatedDate = System.DateTime.Now
            };
            await empContext.EmployeeSalarys.AddAsync(empsalary);
            await empContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        
        [HttpGet]
        public IActionResult GetSalaryData(int EmployeeId)
        {
            var EmpSal = empContext.EmployeeSalarys.Where(x => x.EmployeeId == EmployeeId).ToList();
            
            if (EmpSal != null)
            {
                var data = EmpSal.Select(c => new EmployeeSalary()
                {
                    Id = c.Id,
                    SalaryDate = c.SalaryDate,
                    Amount = c.Amount
                });
                return Json(data);
            }
            else
            {
                return NotFound(); 
            }
        }

    }
}
