namespace ExperimentAgent.API.Persistence;

public class ApplicationContext : DbContext
{
    //dbset
    
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}