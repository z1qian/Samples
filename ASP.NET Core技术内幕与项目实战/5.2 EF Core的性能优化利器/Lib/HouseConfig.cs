using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lib;

internal class HouseConfig : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder.ToTable("T_Houses");
        builder.Property(h => h.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(h => h.Owner)
            .HasMaxLength(50);
        //设置Owner列为并发令牌属性
        //.IsConcurrencyToken();

        builder.Property(h => h.RowVer).IsRowVersion();
    }
}
