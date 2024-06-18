using Training.Core.Model;

namespace Training.Core.Interface;
public interface IGroupTrainingService
{
    Task<IEnumerable<GroupTraining>> GetGroupTrainingsAsync();
    Task<GroupTraining> GetGroupTrainingByIdAsync(int id);
    Task CreateGroupTrainingAsync(GroupTraining groupTraining);
    Task UpdateGroupTrainingAsync(GroupTraining groupTraining);
    Task DeleteGroupTrainingAsync(int id);
}
