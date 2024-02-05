namespace SQLServerAgent.API.Persistence.EntityConfigurations;

public class ProductOrderEntityTypeConfiguration : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        builder.HasKey(productOrder => productOrder.Id);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(productOrder => productOrder.ProductId);
        
        builder.HasIndex(productOrder => productOrder.ProductId)
            .IsUnique(false);

        builder.HasOne<Order>()
            .WithOne()
            .HasForeignKey<ProductOrder>(productOrder => productOrder.OrderId);
        
        builder.HasIndex(productOrder => productOrder.OrderId)
            .IsUnique(false);
    }
}