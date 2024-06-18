using Training.Core.Model;

namespace Training.Core.Interface;
public interface IIndividualTrainingRepository
{
    Task<IEnumerable<IndividualTraining>> GetAllAsync();
    Task<IndividualTraining> GetByIdAsync(int id);
    Task AddAsync(IndividualTraining individualTraining);
    Task UpdateAsync(IndividualTraining individualTraining);
    Task DeleteAsync(int id);
}
