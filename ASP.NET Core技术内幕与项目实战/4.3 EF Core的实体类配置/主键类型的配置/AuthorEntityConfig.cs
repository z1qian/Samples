using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace 主键类型的配置;

internal class AuthorEntityConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("T_Authors");

        builder.Property(a => a.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
