using Training.Core.Model;

namespace Training.Core.Interface;
public interface IGroupTrainingRepository
{
    Task<IEnumerable<GroupTraining>> GetAllAsync();
    Task<GroupTraining> GetByIdAsync(int id);
    Task AddAsync(GroupTraining groupTraining);
    Task UpdateAsync(GroupTraining groupTraining);
    Task DeleteAsync(int id);
}
