using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._6_关系配置;

internal class ArticleConfig : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("T_Articles");

        builder.Property(a => a.Content)
            .IsRequired().IsUnicode();

        builder.Property(a => a.Title)
            .IsRequired().IsUnicode().HasMaxLength(255);
    }

}

