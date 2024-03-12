namespace ExperimentAgent.API.Persistence.EntityConfigurations;

public class ExperimentOutcomeEntityTypeConfiguration : IEntityTypeConfiguration<ExperimentOutcome>
{
    public void Configure(EntityTypeBuilder<ExperimentOutcome> builder)
    {
        builder.HasKey(experimentOutcome => experimentOutcome.Id);
    }
}