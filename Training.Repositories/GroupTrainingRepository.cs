using Microsoft.EntityFrameworkCore;
using Training.Core.Interface;
using Training.Core.Model;
using Training.DataAccess;

namespace Training.Repositories;
public class GroupTrainingRepository : IGroupTrainingRepository
{
    private readonly GymContext _context;

    public GroupTrainingRepository(GymContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GroupTraining>> GetAllAsync()
    {
        return await _context.GroupTrainings.ToListAsync();
    }

    public async Task<GroupTraining> GetByIdAsync(int id)
    {
        return await _context.GroupTrainings.FindAsync(id);
    }

    public async Task AddAsync(GroupTraining groupTraining)
    {
        _context.GroupTrainings.Add(groupTraining);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(GroupTraining groupTraining)
    {
        _context.Entry(groupTraining).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var groupTraining = await _context.GroupTrainings.FindAsync(id);
        if (groupTraining != null)
        {
            _context.GroupTrainings.Remove(groupTraining);
            await _context.SaveChangesAsync();
        }
    }
}

