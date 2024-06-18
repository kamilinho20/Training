using System.Net.Http.Json;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Services;
public class ClientService : IClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("GymApi");
    }

    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Client>>("client");
    }

    public async Task<Client> GetClientByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Client>($"client/{id}");
    }

    public async Task CreateClientAsync(Client client)
    {
        await _httpClient.PostAsJsonAsync("client", client);
    }

    public async Task UpdateClientAsync(Client client)
    {
        await _httpClient.PutAsJsonAsync($"client/{client.Id}", client);
    }

    public async Task DeleteClientAsync(int id)
    {
        await _httpClient.DeleteAsync($"client/{id}");
    }
}