using Training.Core.Model;

namespace Training.Core.Interface;
public interface IClientService
{
    Task<IEnumerable<Client>> GetClientsAsync();
    Task<Client> GetClientByIdAsync(int id);
    Task CreateClientAsync(Client client);
    Task UpdateClientAsync(Client client);
    Task DeleteClientAsync(int id);
}

