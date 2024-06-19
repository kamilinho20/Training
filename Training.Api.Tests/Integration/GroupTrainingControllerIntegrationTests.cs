
namespace Training.Api.Tests.Integration;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Training.Api;
using Training.Core.Model;
using Training.DataAccess;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

public class GroupTrainingControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public GroupTrainingControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task ClearDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymContext>();
        dbContext.GroupTrainings.RemoveRange(dbContext.GroupTrainings);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetGroupTrainings_ReturnsEmptyListInitially()
    {
        // Arrange
        await ClearDatabase();

        // Act
        var response = await _client.GetAsync("/api/GroupTraining");

        // Assert
        response.EnsureSuccessStatusCode();
        var trainings = await response.Content.ReadFromJsonAsync<List<GroupTraining>>();
        Assert.Empty(trainings);
    }

    [Fact]
    public async Task PostGroupTraining_AddsGroupTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new GroupTraining { Name = "Yoga", TrainerId = 1, Date = DateTime.Now };

        // Act
        var response = await _client.PostAsJsonAsync("/api/GroupTraining", training);
        response.EnsureSuccessStatusCode();

        var createdTraining = await response.Content.ReadFromJsonAsync<GroupTraining>();

        // Assert
        Assert.Equal(training.Name, createdTraining.Name);
        Assert.NotEqual(0, createdTraining.Id);
    }

    [Fact]
    public async Task GetGroupTraining_ReturnsGroupTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new GroupTraining { Name = "Pilates", TrainerId = 2, Date = DateTime.Now };
        var postResponse = await _client.PostAsJsonAsync("/api/GroupTraining", training);
        postResponse.EnsureSuccessStatusCode();

        var createdTraining = await postResponse.Content.ReadFromJsonAsync<GroupTraining>();

        // Act
        var getResponse = await _client.GetAsync($"/api/GroupTraining/{createdTraining.Id}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var fetchedTraining = await getResponse.Content.ReadFromJsonAsync<GroupTraining>();
        Assert.Equal(createdTraining.Id, fetchedTraining.Id);
    }

    [Fact]
    public async Task DeleteGroupTraining_DeletesGroupTraining()
    {
        // Arrange
        await ClearDatabase();
        var training = new GroupTraining { Name = "Zumba", TrainerId = 3, Date = DateTime.Now };
        var postResponse = await _client.PostAsJsonAsync("/api/GroupTraining", training);
        postResponse.EnsureSuccessStatusCode();

        var createdTraining = await postResponse.Content.ReadFromJsonAsync<GroupTraining>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/GroupTraining/{createdTraining.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/GroupTraining/{createdTraining.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

