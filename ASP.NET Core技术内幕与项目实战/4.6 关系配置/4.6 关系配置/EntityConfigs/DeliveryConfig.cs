using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._6_关系配置;

internal class DeliveryConfig : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("T_Deliveries");
        builder.Property(d => d.CompanyName)
            .IsUnicode()
            .HasMaxLength(10);
        builder.Property(d => d.Number)
            .HasMaxLength(50);

        //由于在一对一关系中，必须显式地指定外键配置在哪个实体类中，
        //因此我们通过HasForeignKey方法声明外键对应的属性。
        builder.HasOne<Order>(o => o.Order)
            .WithOne(d => d.Delivery)
           .HasForeignKey<Delivery>(d => d.OrderId);
    }
}

