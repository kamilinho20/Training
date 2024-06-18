using Training.Core.Model;

namespace Training.Core.Interface;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task CreateEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
}
