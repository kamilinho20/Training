
namespace Training.Api.Tests.Integration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Training.Api;
using Training.Core.Model;
using Training.DataAccess;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

public class EmployeeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public EmployeeControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task ClearDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymContext>();
        dbContext.Employees.RemoveRange(dbContext.Employees);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetEmployees_ReturnsEmptyListInitially()
    {
        // Arrange
        await ClearDatabase();

        // Act
        var response = await _client.GetAsync("/api/Employee");

        // Assert
        response.EnsureSuccessStatusCode();
        var employees = await response.Content.ReadFromJsonAsync<List<Employee>>();
        Assert.Empty(employees);
    }

    [Fact]
    public async Task PostEmployee_AddsEmployee()
    {
        // Arrange
        await ClearDatabase();
        var employee = new Employee { FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Employee", employee);
        response.EnsureSuccessStatusCode();

        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();

        // Assert
        Assert.Equal(employee.FirstName, createdEmployee.FirstName);
        Assert.NotEqual(0, createdEmployee.Id);
    }

    [Fact]
    public async Task GetEmployee_ReturnsEmployee()
    {
        // Arrange
        await ClearDatabase();
        var employee = new Employee { FirstName = "Jane", LastName = "Doe", Role = "Manager", Email = "jane@example.com", Phone = "987654321" };
        var postResponse = await _client.PostAsJsonAsync("/api/Employee", employee);
        postResponse.EnsureSuccessStatusCode();

        var createdEmployee = await postResponse.Content.ReadFromJsonAsync<Employee>();

        // Act
        var getResponse = await _client.GetAsync($"/api/Employee/{createdEmployee.Id}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var fetchedEmployee = await getResponse.Content.ReadFromJsonAsync<Employee>();
        Assert.Equal(createdEmployee.Id, fetchedEmployee.Id);
    }

    [Fact]
    public async Task DeleteEmployee_DeletesEmployee()
    {
        // Arrange
        await ClearDatabase();
        var employee = new Employee { FirstName = "Mark", LastName = "Twain", Role = "Assistant", Email = "mark@example.com", Phone = "111111111" };
        var postResponse = await _client.PostAsJsonAsync("/api/Employee", employee);
        postResponse.EnsureSuccessStatusCode();

        var createdEmployee = await postResponse.Content.ReadFromJsonAsync<Employee>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/Employee/{createdEmployee.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/Employee/{createdEmployee.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

