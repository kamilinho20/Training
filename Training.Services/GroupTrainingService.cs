using System.Net.Http.Json;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Services;

public class GroupTrainingService : IGroupTrainingService
{
    private readonly HttpClient _httpClient;

    public GroupTrainingService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("GymApi");
    }

    public async Task<IEnumerable<GroupTraining>> GetGroupTrainingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<GroupTraining>>("grouptraining");
    }

    public async Task<GroupTraining> GetGroupTrainingByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<GroupTraining>($"grouptraining/{id}");
    }

    public async Task CreateGroupTrainingAsync(GroupTraining groupTraining)
    {
        await _httpClient.PostAsJsonAsync("grouptraining", groupTraining);
    }

    public async Task UpdateGroupTrainingAsync(GroupTraining groupTraining)
    {
        await _httpClient.PutAsJsonAsync($"grouptraining/{groupTraining.Id}", groupTraining);
    }

    public async Task DeleteGroupTrainingAsync(int id)
    {
        await _httpClient.DeleteAsync($"grouptraining/{id}");
    }
}
