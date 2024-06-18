using Training.Core.Model;

namespace Training.Core.Interface;
public interface IIndividualTrainingService
{
    Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsAsync();
    Task<IndividualTraining> GetIndividualTrainingByIdAsync(int id);
    Task CreateIndividualTrainingAsync(IndividualTraining individualTraining);
    Task UpdateIndividualTrainingAsync(IndividualTraining individualTraining);
    Task DeleteIndividualTrainingAsync(int id);
}
