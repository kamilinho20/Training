using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;
using Training.DataAccess;

namespace Training.Repositories;
public class IndividualTrainingRepository : IIndividualTrainingRepository
{
    private readonly GymContext _context;

    public IndividualTrainingRepository(GymContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<IndividualTraining>> GetAllAsync()
    {
        return await _context.IndividualTrainings.ToListAsync();
    }

    public async Task<IndividualTraining> GetByIdAsync(int id)
    {
        return await _context.IndividualTrainings.FindAsync(id);
    }

    public async Task AddAsync(IndividualTraining individualTraining)
    {
        _context.IndividualTrainings.Add(individualTraining);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(IndividualTraining individualTraining)
    {
        _context.Entry(individualTraining).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var individualTraining = await _context.IndividualTrainings.FindAsync(id);
        if (individualTraining != null)
        {
            _context.IndividualTrainings.Remove(individualTraining);
            await _context.SaveChangesAsync();
        }
    }
}
