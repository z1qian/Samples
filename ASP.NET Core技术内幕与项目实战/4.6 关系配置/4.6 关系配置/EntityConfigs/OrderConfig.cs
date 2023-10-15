using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._6_关系配置;

internal class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("T_Orders");
        builder.Property(o => o.Address).IsUnicode();
        builder.Property(o => o.Name).IsUnicode();

        //builder.HasOne<Delivery>(o => o.Delivery)
        //    .WithOne(d => d.Order)
        //   .HasForeignKey<Delivery>(d => d.OrderId);
    }
}