using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._3_EF_Core的实体类配置;

internal class StudentEntityConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("T_Students");
        builder.HasIndex(s => s.Name);
        builder.Ignore(s => s.Address);

        //链式调用
        builder.ToTable("T_Students")
            .HasIndex(s => s.Name);
        builder.Ignore(s => s.Address);

        builder.Property(s => s.Name)
            .HasMaxLength(255)
            .HasColumnName("StudentName")
            .IsRequired();
    }
}
