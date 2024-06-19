namespace Training.Api.Tests.Integration;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Training.Core.Model;
using Training.DataAccess;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

public class IndividualTrainingControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public IndividualTrainingControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task ClearDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymContext>();
        dbContext.IndividualTrainings.RemoveRange(dbContext.IndividualTrainings);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetIndividualTrainings_ReturnsEmptyListInitially()
    {
        // Arrange
        await ClearDatabase();

        // Act
        var response = await _client.GetAsync("/api/IndividualTraining");

        // Assert
        response.EnsureSuccessStatusCode();
        var trainings = await response.Content.ReadFromJsonAsync<List<IndividualTraining>>();
        Assert.Empty(trainings);
    }

    [Fact]
    public async Task PostIndividualTraining_AddsIndividualTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new IndividualTraining { ClientId = 1, TrainerId = 1, Description = "Training 1", Date = DateTime.Now };

        // Act
        var response = await _client.PostAsJsonAsync("/api/IndividualTraining", training);
        response.EnsureSuccessStatusCode();

        var createdTraining = await response.Content.ReadFromJsonAsync<IndividualTraining>();

        // Assert
        Assert.Equal(training.Description, createdTraining.Description);
        Assert.NotEqual(0, createdTraining.Id);
    }

    [Fact]
    public async Task GetIndividualTraining_ReturnsIndividualTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new IndividualTraining { ClientId = 2, TrainerId = 2, Description = "Training 2", Date = DateTime.Now };
        var postResponse = await _client.PostAsJsonAsync("/api/IndividualTraining", training);
        postResponse.EnsureSuccessStatusCode();

        var createdTraining = await postResponse.Content.ReadFromJsonAsync<IndividualTraining>();

        // Act
        var getResponse = await _client.GetAsync($"/api/IndividualTraining/{createdTraining.Id}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var fetchedTraining = await getResponse.Content.ReadFromJsonAsync<IndividualTraining>();
        Assert.Equal(createdTraining.Id, fetchedTraining.Id);
    }

    [Fact]
    public async Task DeleteIndividualTraining_DeletesIndividualTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new IndividualTraining { ClientId = 3, TrainerId = 3, Description = "Training 3", Date = DateTime.Now };
        var postResponse = await _client.PostAsJsonAsync("/api/IndividualTraining", training);
        postResponse.EnsureSuccessStatusCode();

        var createdTraining = await postResponse.Content.ReadFromJsonAsync<IndividualTraining>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/IndividualTraining/{createdTraining.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/IndividualTraining/{createdTraining.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

