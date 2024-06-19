
namespace Training.Api.Tests.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Training.Api.Controllers;
using Training.Core.Interface;
using Training.Core.Model;
using Xunit;

public class EmployeeControllerTests
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _controller = new EmployeeController(_mockEmployeeRepository.Object);
    }

    [Fact]
    public async Task GetEmployees_ReturnsOkResult_WithListOfEmployees()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Doe", Role = "Manager", Email = "jane@example.com", Phone = "987654321" }
        };
        _mockEmployeeRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(employees);

        // Act
        var result = await _controller.GetEmployees();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnEmployees = Assert.IsType<List<Employee>>(okResult.Value);
        Assert.Equal(2, returnEmployees.Count);
    }

    [Fact]
    public async Task GetEmployee_ReturnsOkResult_WithEmployee()
    {
        // Arrange
        var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(employee);

        // Act
        var result = await _controller.GetEmployee(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnEmployee = Assert.IsType<Employee>(okResult.Value);
        Assert.Equal(employee.Id, returnEmployee.Id);
    }

    [Fact]
    public async Task GetEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(null as Employee);

        // Act
        var result = await _controller.GetEmployee(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostEmployee_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var employee = new Employee { Id = 3, FirstName = "New", LastName = "Employee", Role = "Assistant", Email = "new@example.com", Phone = "555555555" };
        _mockEmployeeRepository.Setup(repo => repo.AddAsync(employee)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PostEmployee(employee);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnEmployee = Assert.IsType<Employee>(createdAtActionResult.Value);
        Assert.Equal(employee.FirstName, returnEmployee.FirstName);
    }

    [Fact]
    public async Task DeleteEmployee_ReturnsNoContentResult()
    {
        // Arrange
        var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(employee);
        _mockEmployeeRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteEmployee(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
    {
        // Arrange
        _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Employee)null);

        // Act
        var result = await _controller.DeleteEmployee(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
