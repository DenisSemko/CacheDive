namespace SQLServerAgent.API.Persistence.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(order => order.UserId);
        
        builder.HasIndex(order => order.UserId)
            .IsUnique(false);
    }
}