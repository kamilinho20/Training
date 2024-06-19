
namespace Training.Repositories.Tests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.Core.Model;
using Training.DataAccess;
using Training.Repositories;
using Xunit;

public class GroupTrainingRepositoryTests
{
    private readonly GymContext _context;
    private readonly GroupTrainingRepository _groupTrainingRepository;

    public GroupTrainingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<GymContext>()
            .UseInMemoryDatabase(databaseName: "GroupTrainingRepositoryTests")
            .Options;
        _context = new GymContext(options);
        _groupTrainingRepository = new GroupTrainingRepository(_context);
    }

    // Cleanup method to clear database before each test
    private void ClearDatabase()
    {
        _context.GroupTrainings.RemoveRange(_context.GroupTrainings);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGroupTrainings()
    {
        // Arrange
        ClearDatabase();
        _context.GroupTrainings.AddRange(new List<GroupTraining>
        {
            new GroupTraining { Name = "Yoga", TrainerId = 1, Date = DateTime.Now },
            new GroupTraining { Name = "Pilates", TrainerId = 2, Date = DateTime.Now }
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _groupTrainingRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGroupTraining()
    {
        // Arrange
        ClearDatabase();
        var training = new GroupTraining { Name = "Yoga", TrainerId = 1, Date = DateTime.Now };
        _context.GroupTrainings.Add(training);
        await _context.SaveChangesAsync();

        // Act
        var result = await _groupTrainingRepository.GetByIdAsync(training.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(training.Id, result.Id);
    }

    [Fact]
    public async Task AddAsync_AddsGroupTraining()
    {
        // Arrange
        ClearDatabase();
        var training = new GroupTraining { Name = "Zumba", TrainerId = 3, Date = DateTime.Now };

        // Act
        await _groupTrainingRepository.AddAsync(training);
        var addedTraining = await _context.GroupTrainings.FindAsync(training.Id);

        // Assert
        Assert.NotNull(addedTraining);
        Assert.Equal(training.Name, addedTraining.Name);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesGroupTraining()
    {
        // Arrange
        ClearDatabase();
        var training = new GroupTraining { Name = "Yoga", TrainerId = 1, Date = DateTime.Now };
        _context.GroupTrainings.Add(training);
        await _context.SaveChangesAsync();

        training.Name = "Advanced Yoga";

        // Act
        await _groupTrainingRepository.UpdateAsync(training);
        var updatedTraining = await _context.GroupTrainings.FindAsync(training.Id);

        // Assert
        Assert.NotNull(updatedTraining);
        Assert.Equal("Advanced Yoga", updatedTraining.Name);
    }

    [Fact]
    public async Task DeleteAsync_DeletesGroupTraining()
    {
        // Arrange
        ClearDatabase();
        var training = new GroupTraining { Name = "Yoga", TrainerId = 1, Date = DateTime.Now };
        _context.GroupTrainings.Add(training);
        await _context.SaveChangesAsync();

        // Act
        await _groupTrainingRepository.DeleteAsync(training.Id);
        var deletedTraining = await _context.GroupTrainings.FindAsync(training.Id);

        // Assert
        Assert.Null(deletedTraining);
    }
}
