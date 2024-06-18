namespace Training.Core.Model;
public class GroupTraining
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TrainerId { get; set; }
    public Employee Trainer { get; set; }
    public DateTime Date { get; set; }
    public List<Client> Clients { get; set; }
}
