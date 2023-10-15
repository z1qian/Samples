using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreLib;

//配置实体类和数据库表的对应关系
internal class BookEntityConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("T_Books");

        builder.Property(e => e.Title).HasMaxLength(50).IsRequired();
        builder.Property(e => e.AuthorName).HasMaxLength(20).IsRequired();
        builder.Property(e => e.Remark).HasMaxLength(100).IsRequired().HasDefaultValue("暂无备注");
    }
}
