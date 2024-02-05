namespace SQLServerAgent.API.Persistence.EntityConfigurations;

public class ProductBasketEntityTypeConfiguration : IEntityTypeConfiguration<ProductBasket>
{
    public void Configure(EntityTypeBuilder<ProductBasket> builder)
    {
        builder.HasKey(productBasket => productBasket.Id);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(productBasket => productBasket.ProductId);
        
        builder.HasIndex(productBasket => productBasket.ProductId)
            .IsUnique(false);

        builder.HasOne<Basket>()
            .WithOne()
            .HasForeignKey<ProductBasket>(productBasket => productBasket.BasketId);
        
        builder.HasIndex(productBasket => productBasket.BasketId)
            .IsUnique(false);
    }
}