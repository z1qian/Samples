using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore中实现充血模型;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("T_Users");
        //特征三
        builder.Property("passwordHash");
        //特征四
        builder.Property(u => u.Remark).HasField("remark");
        //特征五
        builder.Ignore(u => u.Tag);
    }
}
