using Microsoft.EntityFrameworkCore;
using Training.Core.Model;

namespace Training.DataAccess;
public class GymContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<IndividualTraining> IndividualTrainings { get; set; }
    public DbSet<GroupTraining> GroupTrainings { get; set; }

    public GymContext(DbContextOptions<GymContext> options) : base(options)
    {
    }
}
