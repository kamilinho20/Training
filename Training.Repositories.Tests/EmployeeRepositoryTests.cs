namespace Training.Repositories.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Training.Core.Model;
    using Training.DataAccess;
    using Training.Repositories;
    using Xunit;

    public class EmployeeRepositoryTests
    {
        private readonly GymContext _context;
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GymContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeRepositoryTests")
                .Options;
            _context = new GymContext(options);
            _employeeRepository = new EmployeeRepository(_context);
        }

        private void ClearDatabase()
        {
            _context.GroupTrainings.RemoveRange(_context.GroupTrainings);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEmployees()
        {
            // Arrange
            ClearDatabase();
            _context.Employees.AddRange(new List<Employee>
        {
            new Employee { FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" },
            new Employee { FirstName = "Jane", LastName = "Doe", Role = "Manager", Email = "jane@example.com", Phone = "987654321" }
        });
            await _context.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEmployee()
        {
            // Arrange
            ClearDatabase();
            var employee = new Employee { FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetByIdAsync(employee.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.Id, result.Id);
        }

        [Fact]
        public async Task AddAsync_AddsEmployee()
        {
            // Arrange
            ClearDatabase();
            var employee = new Employee { FirstName = "New", LastName = "Employee", Role = "Assistant", Email = "new@example.com", Phone = "555555555" };

            // Act
            await _employeeRepository.AddAsync(employee);
            var addedEmployee = await _context.Employees.FindAsync(employee.Id);

            // Assert
            Assert.NotNull(addedEmployee);
            Assert.Equal(employee.FirstName, addedEmployee.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEmployee()
        {
            // Arrange
            ClearDatabase();
            var employee = new Employee { FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            employee.FirstName = "Updated";

            // Act
            await _employeeRepository.UpdateAsync(employee);
            var updatedEmployee = await _context.Employees.FindAsync(employee.Id);

            // Assert
            Assert.NotNull(updatedEmployee);
            Assert.Equal("Updated", updatedEmployee.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEmployee()
        {
            // Arrange
            ClearDatabase();
            var employee = new Employee { FirstName = "John", LastName = "Doe", Role = "Trainer", Email = "john@example.com", Phone = "123456789" };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Act
            await _employeeRepository.DeleteAsync(employee.Id);
            var deletedEmployee = await _context.Employees.FindAsync(employee.Id);

            // Assert
            Assert.Null(deletedEmployee);
        }
    }

}
