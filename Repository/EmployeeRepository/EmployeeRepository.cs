
using EMS_Project.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EMS_Project.Repository.Employee
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        //injecting AppDbContext to Constructor
        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //Add Employee Logic
        public async Task AddEmployee(Models.Employee employee)
        {
            var NewEmployee = new Models.Employee
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeeJob = employee.EmployeeJob
            };
            await _appDbContext.Employees.AddAsync(NewEmployee);
            await _appDbContext.SaveChangesAsync();

        }

        //Get All Employee List Logic
        public async Task<List<Models.Employee>> GetAllEmployees()
        {
            var Employees = await _appDbContext.Employees.ToListAsync();
            List<Models.Employee> employees = new List<Models.Employee>();
            foreach (var employee in Employees)
            {
                var result = new Models.Employee
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    EmployeeEmail = employee.EmployeeEmail,
                    EmployeeJob = employee.EmployeeJob
                };
                employees.Add(result);
            }
            return employees;
        }

        //Delete Employee using EmployeeID
        public async Task<bool> DeleteEmployee(string id)
        {
            var Employee = await _appDbContext.Employees.FindAsync(id);

            if (Employee == null)
            {
                return false;
            }
            else
            {
                _appDbContext.Employees.Remove(Employee);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
        }

        //Get EMployee by ID
        public async Task<Models.Employee> GetEmployeeById(string id)
        {
            var Employee = await _appDbContext.Employees.FindAsync(id);
            var result = new Models.Employee
            {
                EmployeeId = Employee.EmployeeId,
                EmployeeName = Employee.EmployeeName,
                EmployeeEmail = Employee.EmployeeEmail,
                EmployeeJob = Employee.EmployeeJob
            };

            return result;
        }

        //Update Employee Details
        public async Task<bool> UpdateEmployee(Models.Employee employee)
        {
            var EditEmployee = await _appDbContext.Employees.FindAsync(employee.EmployeeId);

            if (EditEmployee == null)
            {
                return false;
            }
            else
            {
                EditEmployee.EmployeeName = employee.EmployeeName;
                EditEmployee.EmployeeEmail = employee.EmployeeEmail;
                EditEmployee.EmployeeJob = employee.EmployeeJob;
                await _appDbContext.SaveChangesAsync();
                return true;
            }
        }


        //Calculateing working Days
        public async Task<int> CalculateWorkDates(DateOnly startDate, DateOnly endDate)
        {
            var Hoildays = await _appDbContext.Holidays.Select(h => (h.Holiday1)).ToListAsync();

            int workingDays = 0;

            if (startDate < endDate)
            {
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || Hoildays.Contains(date))
                    {
                        continue;
                    }
                    else
                    {
                        workingDays++;
                    }
                }

            }
            return workingDays;

        }
    }
}
