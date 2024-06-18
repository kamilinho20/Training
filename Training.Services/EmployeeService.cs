using System.Net.Http.Json;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Services;
public class EmployeeService : IEmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("GymApi");
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Employee>>("employee");
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Employee>($"employee/{id}");
    }

    public async Task CreateEmployeeAsync(Employee employee)
    {
        await _httpClient.PostAsJsonAsync("employee", employee);
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        await _httpClient.PutAsJsonAsync($"employee/{employee.Id}", employee);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        await _httpClient.DeleteAsync($"employee/{id}");
    }
}
