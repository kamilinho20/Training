using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GroupTrainingController : ControllerBase
{
    private readonly IGroupTrainingRepository _groupTrainingRepository;

    public GroupTrainingController(IGroupTrainingRepository groupTrainingRepository)
    {
        _groupTrainingRepository = groupTrainingRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupTraining>>> GetGroupTrainings()
    {
        return Ok(await _groupTrainingRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupTraining>> GetGroupTraining(int id)
    {
        var groupTraining = await _groupTrainingRepository.GetByIdAsync(id);
        if (groupTraining == null)
        {
            return NotFound();
        }
        return Ok(groupTraining);
    }

    [HttpPost]
    public async Task<ActionResult<GroupTraining>> PostGroupTraining(GroupTraining groupTraining)
    {
        await _groupTrainingRepository.AddAsync(groupTraining);
        return CreatedAtAction("GetGroupTraining", new { id = groupTraining.Id }, groupTraining);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroupTraining(int id, GroupTraining groupTraining)
    {
        if (id != groupTraining.Id)
        {
            return BadRequest();
        }

        try
        {
            await _groupTrainingRepository.UpdateAsync(groupTraining);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(await GroupTrainingExists(id)))
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
    public async Task<IActionResult> DeleteGroupTraining(int id)
    {
        var groupTraining = await _groupTrainingRepository.GetByIdAsync(id);
        if (groupTraining == null)
        {
            return NotFound();
        }

        await _groupTrainingRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> GroupTrainingExists(int id)
    {
        return (await _groupTrainingRepository.GetByIdAsync(id)) != null;
    }
}

