using MediaEncoder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediaEncoder.Infrastructure.Configs;

class EncodingItemConfig : IEntityTypeConfiguration<EncodingItem>
{
    public void Configure(EntityTypeBuilder<EncodingItem> builder)
    {
        builder.ToTable("T_ME_EncodingItems");

        builder.HasKey(e => e.Id).IsClustered(false);
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.FileSHA256Hash).HasMaxLength(64).IsUnicode(false);
        builder.Property(e => e.OutputFormat).HasMaxLength(10).IsUnicode(false);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(10);

        builder.HasIndex(e => new { e.FileSHA256Hash, e.FileSizeInBytes, e.Status });
        //如果你经常需要根据Status字段来查询数据，
        //并且这些查询不包括FileSHA256Hash和FileSizeInBytes字段，
        //那么一个单独的Status索引可能会提高这些查询的性能
        builder.HasIndex(e => e.Status);
    }
}