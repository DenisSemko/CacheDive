namespace SQLServerAgent.API.Persistence.EntityConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(product => product.CategoryId);
        
        builder.HasIndex(product => product.CategoryId)
            .IsUnique(false);
    }
}