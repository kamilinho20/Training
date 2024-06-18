namespace Training.Core.Model;
public class IndividualTraining
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; }
    public int TrainerId { get; set; }
    public Employee Trainer { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
}
