
namespace Training.Api.Tests.Integration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Training.Core.Model;
using Xunit;

public class ClientControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ClientControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetClients_ReturnsClients()
    {
        // Arrange
        var client = new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };
        var client2 = new Client { FirstName = "Justin", LastName = "Clooney", Email = "justin@example.com", Phone = "987654321" };
        await _client.PostAsJsonAsync("/api/Client", client);
        await _client.PostAsJsonAsync("/api/Client", client2);
        
        // Act
        var response = await _client.GetAsync("/api/Client");

        // Assert
        response.EnsureSuccessStatusCode();
        var clients = await response.Content.ReadFromJsonAsync<List<Client>>();
        Assert.NotEmpty(clients);
        Assert.True(clients.Count() >= 2);
    }

    [Fact]
    public async Task PostClient_AddsClient()
    {
        // Arrange
        var client = new Client { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Client", client);
        response.EnsureSuccessStatusCode();

        var createdClient = await response.Content.ReadFromJsonAsync<Client>();

        // Assert
        Assert.Equal(client.FirstName, createdClient.FirstName);
        Assert.NotEqual(0, createdClient.Id);
    }

    [Fact]
    public async Task GetClient_ReturnsClient()
    {
        // Arrange
        var client = new Client { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", Phone = "987654321" };
        var postResponse = await _client.PostAsJsonAsync("/api/Client", client);
        postResponse.EnsureSuccessStatusCode();

        var createdClient = await postResponse.Content.ReadFromJsonAsync<Client>();

        // Act
        var getResponse = await _client.GetAsync($"/api/Client/{createdClient.Id}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var fetchedClient = await getResponse.Content.ReadFromJsonAsync<Client>();
        Assert.Equal(createdClient.Id, fetchedClient.Id);
    }

    [Fact]
    public async Task DeleteClient_DeletesClient()
    {
        // Arrange
        var client = new Client { FirstName = "Mark", LastName = "Twain", Email = "mark@example.com", Phone = "111111111" };
        var postResponse = await _client.PostAsJsonAsync("/api/Client", client);
        postResponse.EnsureSuccessStatusCode();

        var createdClient = await postResponse.Content.ReadFromJsonAsync<Client>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/Client/{createdClient.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/Client/{createdClient.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
