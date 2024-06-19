
namespace Training.Api.Tests.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Training.Api.Controllers;
using Training.Core.Interface;
using Training.Core.Model;
using Xunit;

public class IndividualTrainingControllerTests
{
    private readonly Mock<IIndividualTrainingRepository> _mockIndividualTrainingRepository;
    private readonly IndividualTrainingController _controller;

    public IndividualTrainingControllerTests()
    {
        _mockIndividualTrainingRepository = new Mock<IIndividualTrainingRepository>();
        _controller = new IndividualTrainingController(_mockIndividualTrainingRepository.Object);
    }

    [Fact]
    public async Task GetIndividualTrainings_ReturnsOkResult_WithListOfIndividualTrainings()
    {
        // Arrange
        var trainings = new List<IndividualTraining>
    {
        new IndividualTraining { Id = 1, ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now },
        new IndividualTraining { Id = 2, ClientId = 2, TrainerId = 2, Description = "Training 2", Date = DateTime.Now }
    };
        _mockIndividualTrainingRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(trainings);

        // Act
        var result = await _controller.GetIndividualTrainings();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnTrainings = Assert.IsType<List<IndividualTraining>>(okResult.Value);
        Assert.Equal(2, returnTrainings.Count);
    }

    [Fact]
    public async Task GetIndividualTraining_ReturnsOkResult_WithIndividualTraining()
    {
        // Arrange
        var training = new IndividualTraining { Id = 1, ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };
        _mockIndividualTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(training);

        // Act
        var result = await _controller.GetIndividualTraining(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnTraining = Assert.IsType<IndividualTraining>(okResult.Value);
        Assert.Equal(training.Id, returnTraining.Id);
    }

    [Fact]
    public async Task GetIndividualTraining_ReturnsNotFound_WhenIndividualTrainingDoesNotExist()
    {
        // Arrange
        _mockIndividualTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(null as IndividualTraining);

        // Act
        var result = await _controller.GetIndividualTraining(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostIndividualTraining_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var training = new IndividualTraining { Id = 3, ClientId = 3, TrainerId = 3, Description = "Training 3", Date = DateTime.Now };
        _mockIndividualTrainingRepository.Setup(repo => repo.AddAsync(training)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PostIndividualTraining(training);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnTraining = Assert.IsType<IndividualTraining>(createdAtActionResult.Value);
        Assert.Equal(training.Description, returnTraining.Description);
    }

    [Fact]
    public async Task DeleteIndividualTraining_ReturnsNoContentResult()
    {
        // Arrange
        var training = new IndividualTraining { Id = 1, ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };
        _mockIndividualTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(training);
        _mockIndividualTrainingRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteIndividualTraining(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteIndividualTraining_ReturnsNotFound_WhenIndividualTrainingDoesNotExist()
    {
        // Arrange
        _mockIndividualTrainingRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(null as IndividualTraining);

        // Act
        var result = await _controller.DeleteIndividualTraining(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

