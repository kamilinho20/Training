using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return Ok(await _clientRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Client>> PostClient(Client client)
    {
        await _clientRepository.AddAsync(client);
        return CreatedAtAction("GetClient", new { id = client.Id }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutClient(int id, Client client)
    {
        if (id != client.Id)
        {
            return BadRequest();
        }

        try
        {
            await _clientRepository.UpdateAsync(client);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(await ClientExists(id)))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _clientRepository.GetByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        await _clientRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> ClientExists(int id)
    {
        return (await _clientRepository.GetByIdAsync(id)) != null;
    }
}
