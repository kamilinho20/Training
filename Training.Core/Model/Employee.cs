namespace Training.Core.Model;
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; } // e.g., Trainer, Receptionist
    public string Email { get; set; }
    public string Phone { get; set; }
}
