namespace SQLServerAgent.API.Persistence.EntityConfigurations;

public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasKey(basket => basket.Id);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(basket => basket.UserId);
        
        builder.HasIndex(basket => basket.UserId)
            .IsUnique(false);
    }
}