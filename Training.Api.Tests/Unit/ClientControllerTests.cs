
namespace Training.Api.Tests.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Training.Core.Interface;
using Training.Core.Model;
using Xunit;

public class ClientControllerTests
{
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly ClientController _controller;

    public ClientControllerTests()
    {
        _mockClientRepository = new Mock<IClientRepository>();
        _controller = new ClientController(_mockClientRepository.Object);
    }

    [Fact]
    public async Task GetClients_ReturnsOkResult_WithListOfClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" },
            new Client { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", Phone = "987654321" }
        };
        _mockClientRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(clients);

        // Act
        var result = await _controller.GetClients();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnClients = Assert.IsType<List<Client>>(okResult.Value);
        Assert.Equal(2, returnClients.Count);
    }

    [Fact]
    public async Task GetClient_ReturnsOkResult_WithClient()
    {
        // Arrange
        var client = new Client { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(client);

        // Act
        var result = await _controller.GetClient(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnClient = Assert.IsType<Client>(okResult.Value);
        Assert.Equal(client.Id, returnClient.Id);
    }

    [Fact]
    public async Task GetClient_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Client)null);

        // Act
        var result = await _controller.GetClient(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostClient_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var client = new Client { Id = 3, FirstName = "New", LastName = "Client", Email = "new@example.com", Phone = "555555555" };
        _mockClientRepository.Setup(repo => repo.AddAsync(client)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PostClient(client);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnClient = Assert.IsType<Client>(createdAtActionResult.Value);
        Assert.Equal(client.FirstName, returnClient.FirstName);
    }

    [Fact]
    public async Task DeleteClient_ReturnsNoContentResult()
    {
        // Arrange
        var client = new Client { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(client);
        _mockClientRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteClient(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteClient_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _mockClientRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Client)null);

        // Act
        var result = await _controller.DeleteClient(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
