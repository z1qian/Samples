using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore中实现值对象;

class RegionConfig : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        //我们用OwnsOne对从属实体类型属性进行配置
        builder.OwnsOne(c => c.Area, nb =>
        {
            //让枚举类型在数据库中映射为string类型而不是默认的int类型
            nb.Property(e => e.Unit).HasMaxLength(20)
            .IsUnicode(false).HasConversion<string>();
        });

        builder.OwnsOne(c => c.Location);

        builder.Property(c => c.Level).HasMaxLength(20)
 .IsUnicode(false).HasConversion<string>();

        builder.OwnsOne(c => c.Name, nb =>
        {
            nb.Property(e => e.English).HasMaxLength(20).IsUnicode(false);
            nb.Property(e => e.Chinese).HasMaxLength(20).IsUnicode(true);
        });
    }
}