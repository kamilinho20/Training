
namespace Training.Repositories.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Training.Core.Model;
    using Training.DataAccess;
    using Training.Repositories;
    using Xunit;

    public class IndividualTrainingRepositoryTests
    {
        private readonly GymContext _context;
        private readonly IndividualTrainingRepository _individualTrainingRepository;

        public IndividualTrainingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GymContext>()
                .UseInMemoryDatabase(databaseName: "IndividualTrainingRepositoryTests")
                .Options;
            _context = new GymContext(options);
            _individualTrainingRepository = new IndividualTrainingRepository(_context);
        }

        private void ClearDatabase()
        {
            _context.GroupTrainings.RemoveRange(_context.GroupTrainings);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllIndividualTrainings()
        {
            // Arrange
            ClearDatabase();
            _context.IndividualTrainings.AddRange(new List<IndividualTraining>
        {
            new IndividualTraining { ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now },
            new IndividualTraining { ClientId = 2, TrainerId = 2, Description = "Training 2", Date = DateTime.Now }
        });
            await _context.SaveChangesAsync();

            // Act
            var result = await _individualTrainingRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsIndividualTraining()
        {
            // Arrange
            ClearDatabase();
            var training = new IndividualTraining { ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };
            _context.IndividualTrainings.Add(training);
            await _context.SaveChangesAsync();

            // Act
            var result = await _individualTrainingRepository.GetByIdAsync(training.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(training.Id, result.Id);
        }

        [Fact]
        public async Task AddAsync_AddsIndividualTraining()
        {
            // Arrange
            ClearDatabase();
            var training = new IndividualTraining { ClientId = 3, TrainerId = 3, Description = "Training 3", Date = DateTime.Now };

            // Act
            await _individualTrainingRepository.AddAsync(training);
            var addedTraining = await _context.IndividualTrainings.FindAsync(training.Id);

            // Assert
            Assert.NotNull(addedTraining);
            Assert.Equal(training.Description, addedTraining.Description);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesIndividualTraining()
        {
            // Arrange
            ClearDatabase();
            var training = new IndividualTraining { ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };
            _context.IndividualTrainings.Add(training);
            await _context.SaveChangesAsync();

            training.Description = "Updated Training";

            // Act
            await _individualTrainingRepository.UpdateAsync(training);
            var updatedTraining = await _context.IndividualTrainings.FindAsync(training.Id);

            // Assert
            Assert.NotNull(updatedTraining);
            Assert.Equal("Updated Training", updatedTraining.Description);
        }

        [Fact]
        public async Task DeleteAsync_DeletesIndividualTraining()
        {
            // Arrange
            ClearDatabase();
            var training = new IndividualTraining { ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };
            _context.IndividualTrainings.Add(training);
            await _context.SaveChangesAsync();

            // Act
            await _individualTrainingRepository.DeleteAsync(training.Id);
            var deletedTraining = await _context.IndividualTrainings.FindAsync(training.Id);

            // Assert
            Assert.Null(deletedTraining);
        }
    }

}
