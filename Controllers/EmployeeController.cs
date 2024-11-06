using EMS_Project.Models;
using EMS_Project.Repository.Employee;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _IemployeeRepository;

        //Injecting Services to controller
        public EmployeeController(ILogger<EmployeeController> logger,IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _IemployeeRepository = employeeRepository;
        }


        //Get Method for AddEmployee
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }


        //Post Method for AddEmployee
        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee newEmp)
        {
            
            try
            {
                await _IemployeeRepository.AddEmployee(newEmp);
                return RedirectToAction("AddEmployee");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Employee");
            }
            return View(newEmp);
           
        }


        //Get Method for EmployeeDetails
        [HttpGet]
        public async Task<IActionResult> EmployeeDetails()
        {
            var Employees = await _IemployeeRepository.GetAllEmployees();
            return View(Employees);

        }


        //Get Method for DeleteEmployee using EmployeeId
        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _IemployeeRepository.DeleteEmployee(id);
            if (result == false)
            {
                return NotFound();
            }
            return RedirectToAction("EmployeeDetails");
        }


        //Get Method for EditEmployee using EmployeeId to show the Employee details
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var Employee = await _IemployeeRepository.GetEmployeeById(id);

            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
            //return View();
        }


        //Post Method for EditEmployee to update the Employee details
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            var result = await _IemployeeRepository.UpdateEmployee(employee);
            if (result)
            {
                return RedirectToAction("EmployeeDetails");
            }
            return View(employee);
        }


        //Calculate Work Days Get Method
        [HttpGet]
        public  IActionResult WorkDayCalculator()
        {
            return View();
        }

        //Calculate Work Days Post Method
        [HttpPost]
        public async Task<IActionResult> WorkDayCalculator(DateOnly StartDate,DateOnly EndDate)
        {
            var Days = await _IemployeeRepository.CalculateWorkDates(StartDate, EndDate);
            ViewBag.Days = Days;
            return View();
        }

    }
}
