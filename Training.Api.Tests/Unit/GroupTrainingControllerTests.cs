
namespace Training.Api.Tests.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Training.Api.Controllers;
using Training.Core.Interface;
using Training.Core.Model;
using Xunit;

public class GroupTrainingControllerTests
{
    private readonly Mock<IGroupTrainingRepository> _mockGroupTrainingRepository;
    private readonly GroupTrainingController _controller;

    public GroupTrainingControllerTests()
    {
        _mockGroupTrainingRepository = new Mock<IGroupTrainingRepository>();
        _controller = new GroupTrainingController(_mockGroupTrainingRepository.Object);
    }

    [Fact]
    public async Task GetGroupTrainings_ReturnsOkResult_WithListOfGroupTrainings()
    {
        // Arrange
        var trainings = new List<GroupTraining>
        {
            new GroupTraining { Id = 1, Name = "Yoga", TrainerId = 1, Date = DateTime.Now },
            new GroupTraining { Id = 2, Name = "Pilates", TrainerId = 2, Date = DateTime.Now }
        };
        _mockGroupTrainingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(trainings);

        // Act
        var result = await _controller.GetGroupTrainings();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnTrainings = Assert.IsType<List<GroupTraining>>(okResult.Value);
        Assert.Equal(2, returnTrainings.Count);
    }

    [Fact]
    public async Task GetGroupTraining_ReturnsOkResult_WithGroupTraining()
    {
        // Arrange
        var training = new GroupTraining { Id = 1, Name = "Yoga", TrainerId = 1, Date = DateTime.Now };
        _mockGroupTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(training);

        // Act
        var result = await _controller.GetGroupTraining(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnTraining = Assert.IsType<GroupTraining>(okResult.Value);
        Assert.Equal(training.Id, returnTraining.Id);
    }

    [Fact]
    public async Task GetGroupTraining_ReturnsNotFound_WhenGroupTrainingDoesNotExist()
    {
        // Arrange
        _mockGroupTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(null as GroupTraining);

        // Act
        var result = await _controller.GetGroupTraining(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostGroupTraining_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var training = new GroupTraining { Id = 3, Name = "Zumba", TrainerId = 3, Date = DateTime.Now };
        _mockGroupTrainingRepository.Setup(repo => repo.AddAsync(training)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PostGroupTraining(training);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnTraining = Assert.IsType<GroupTraining>(createdAtActionResult.Value);
        Assert.Equal(training.Name, returnTraining.Name);
    }

    [Fact]
    public async Task DeleteGroupTraining_ReturnsNoContentResult()
    {
        // Arrange
        var training = new GroupTraining { Id = 1, Name = "Yoga", TrainerId = 1, Date = DateTime.Now };
        _mockGroupTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(training);
        _mockGroupTrainingRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGroupTraining(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGroupTraining_ReturnsNotFound_WhenGroupTrainingDoesNotExist()
    {
        // Arrange
        _mockGroupTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(null as GroupTraining);

        // Act
        var result = await _controller.DeleteGroupTraining(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

