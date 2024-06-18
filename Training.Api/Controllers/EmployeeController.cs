using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;

namespace Training.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return Ok(await _employeeRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
    {
        await _employeeRepository.AddAsync(employee);
        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }

        try
        {
            await _employeeRepository.UpdateAsync(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(await EmployeeExists(id)))
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
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        await _employeeRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> EmployeeExists(int id)
    {
        return (await _employeeRepository.GetByIdAsync(id)) != null;
    }
}

