using System.Net.Http.Json;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Services;
public class IndividualTrainingService : IIndividualTrainingService
{
    private readonly HttpClient _httpClient;

    public IndividualTrainingService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("GymApi");
    }

    public async Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<IndividualTraining>>("individualtraining");
    }

    public async Task<IndividualTraining> GetIndividualTrainingByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<IndividualTraining>($"individualtraining/{id}");
    }

    public async Task CreateIndividualTrainingAsync(IndividualTraining individualTraining)
    {
        await _httpClient.PostAsJsonAsync("individualtraining", individualTraining);
    }

    public async Task UpdateIndividualTrainingAsync(IndividualTraining individualTraining)
    {
        await _httpClient.PutAsJsonAsync($"individualtraining/{individualTraining.Id}", individualTraining);
    }

    public async Task DeleteIndividualTrainingAsync(int id)
    {
        await _httpClient.DeleteAsync($"individualtraining/{id}");
    }
}