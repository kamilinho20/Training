namespace Training.Repositories.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Training.Core.Model;
    using Training.DataAccess;
    using Training.Repositories;
    using Xunit;

    public class ClientRepositoryTests
    {
        private readonly GymContext _context;
        private readonly ClientRepository _clientRepository;

        public ClientRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<GymContext>()
                .UseInMemoryDatabase(databaseName: "ClientRepositoryTests")
                .Options;
            _context = new GymContext(options);
            _clientRepository = new ClientRepository(_context);
        }

        private void ClearDatabase()
        {
            _context.GroupTrainings.RemoveRange(_context.GroupTrainings);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllClients()
        {
            // Arrange
            ClearDatabase();
            _context.Clients.AddRange(new List<Client>
        {
            new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" },
            new Client { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", Phone = "987654321" }
        });
            await _context.SaveChangesAsync();

            // Act
            var result = await _clientRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsClient()
        {
            // Arrange
            ClearDatabase();
            var client = new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            var result = await _clientRepository.GetByIdAsync(client.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.Id, result.Id);
        }

        [Fact]
        public async Task AddAsync_AddsClient()
        {
            // Arrange
            ClearDatabase();
            var client = new Client { FirstName = "New", LastName = "Client", Email = "new@example.com", Phone = "555555555" };

            // Act
            await _clientRepository.AddAsync(client);
            var addedClient = await _context.Clients.FindAsync(client.Id);

            // Assert
            Assert.NotNull(addedClient);
            Assert.Equal(client.FirstName, addedClient.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesClient()
        {
            // Arrange
            ClearDatabase();
            var client = new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            client.FirstName = "Updated";

            // Act
            await _clientRepository.UpdateAsync(client);
            var updatedClient = await _context.Clients.FindAsync(client.Id);

            // Assert
            Assert.NotNull(updatedClient);
            Assert.Equal("Updated", updatedClient.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesClient()
        {
            // Arrange
            ClearDatabase();
            var client = new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Act
            await _clientRepository.DeleteAsync(client.Id);
            var deletedClient = await _context.Clients.FindAsync(client.Id);

            // Assert
            Assert.Null(deletedClient);
        }
    }


}
