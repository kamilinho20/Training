using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class IndividualTrainingController : ControllerBase
{
    private readonly IIndividualTrainingRepository _individualTrainingRepository;

    public IndividualTrainingController(IIndividualTrainingRepository individualTrainingRepository)
    {
        _individualTrainingRepository = individualTrainingRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndividualTraining>>> GetIndividualTrainings()
    {
        return Ok(await _individualTrainingRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IndividualTraining>> GetIndividualTraining(int id)
    {
        var individualTraining = await _individualTrainingRepository.GetByIdAsync(id);
        if (individualTraining == null)
        {
            return NotFound();
        }
        return Ok(individualTraining);
    }

    [HttpPost]
    public async Task<ActionResult<IndividualTraining>> PostIndividualTraining(IndividualTraining individualTraining)
    {
        await _individualTrainingRepository.AddAsync(individualTraining);
        return CreatedAtAction("GetIndividualTraining", new { id = individualTraining.Id }, individualTraining);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutIndividualTraining(int id, IndividualTraining individualTraining)
    {
        if (id != individualTraining.Id)
        {
            return BadRequest();
        }

        try
        {
            await _individualTrainingRepository.UpdateAsync(individualTraining);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(await IndividualTrainingExists(id)))
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
    public async Task<IActionResult> DeleteIndividualTraining(int id)
    {
        var individualTraining = await _individualTrainingRepository.GetByIdAsync(id);
        if (individualTraining == null)
        {
            return NotFound();
        }

        await _individualTrainingRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> IndividualTrainingExists(int id)
    {
        return (await _individualTrainingRepository.GetByIdAsync(id)) != null;
    }
}

