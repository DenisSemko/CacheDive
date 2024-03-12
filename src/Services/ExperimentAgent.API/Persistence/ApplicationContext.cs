namespace ExperimentAgent.API.Persistence;

public class ApplicationContext : DbContext
{
    public DbSet<ExperimentOutcome> ExperimentOutcomes { get; set; }
    
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}