

using EMS_Project.Models;

namespace EMS_Project.Repository.Employee
{
    public interface IEmployeeRepository
    {
        //Add Employee Logic
        Task AddEmployee(Models.Employee employee);

        //Get All Employee List Logic
        Task<List<Models.Employee>> GetAllEmployees();

        //Get Employee Details by Id
        Task<Models.Employee> GetEmployeeById(string id);

        //Delete Employee By ID
        Task <bool> DeleteEmployee(string id);

        // Update Employee Details
        Task <bool> UpdateEmployee(Models.Employee employee);

        //Calculating Working Dates
        Task<int> CalculateWorkDates(DateOnly startDate, DateOnly endDate);

    }
}
