using EMS_Project.Models;
using EMS_Project.Repository.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace EMS_Project.Controllers
{
    
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _IemployeeRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string key = "EmployeeKey";

        //Injecting Services to controller
        public EmployeeController(ILogger<EmployeeController> logger,IEmployeeRepository employeeRepository,IMemoryCache memoryCache)
        {
            _logger = logger;
            _IemployeeRepository = employeeRepository;
            _memoryCache = memoryCache;
        }


        //Get Method for AddEmployee
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }


        //Post Method for AddEmployee
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody]Employee newEmp)
        {
            if (newEmp == null)
            {
                return BadRequest("Employee data cannot be null.");
            }

            try
            {
                await _IemployeeRepository.AddEmployee(newEmp);
                _logger.LogInformation("Employee added successfully: {@Employee}", newEmp);
                //return RedirectToAction("AddEmployee");
                return CreatedAtAction(nameof(AddEmployee), new { id = newEmp.EmployeeId }, newEmp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Employee");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the employee.");
            }
           // return View(newEmp);

        }


        //Get Method for EmployeeDetails
        [HttpGet]
        public async Task<IActionResult> EmployeeDetails()
        {
            // Data fetching message from cache or DB
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Employee> employees;

            if (_memoryCache.TryGetValue(key, out employees))
            {
                _logger.Log(LogLevel.Information, "Data fetched from cache");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Data not fetched from cache");
                employees = await _IemployeeRepository.GetAllEmployees();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                    SlidingExpiration = TimeSpan.FromSeconds(45),
                    Priority = CacheItemPriority.Normal
                };

                _memoryCache.Set(key, employees, cacheEntryOptions);
            }

            stopwatch.Stop();
            _logger.Log(LogLevel.Information, "Time taken to fetch data: " + stopwatch.ElapsedMilliseconds);
            //    return View(Employees);
            // Return employees as JSON
            return Ok(employees);
        }


        //Cache Clear Method
        public IActionResult ClearCache()
        {
            _memoryCache.Remove(key);
            _logger.Log(LogLevel.Information, "Cleared cache");
            return RedirectToAction("EmployeeDetails");

        }
        //Get Method for DeleteEmployee using EmployeeId
        //[HttpGet]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _IemployeeRepository.DeleteEmployee(id);
            if (result == false)
            {
                return NotFound();
            }
            //return RedirectToAction("EmployeeDetails");
            return Ok();
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
            //return View(Employee);
            return Ok(Employee);

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
            //return View(employee);
            return Ok();
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
