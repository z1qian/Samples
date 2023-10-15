using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace _4._6_关系配置;

internal class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("T_Students");
        builder.Property(s => s.Name)
            .IsUnicode()
            .HasMaxLength(20);

        builder.HasMany<Teacher>(s => s.Teachers)
            .WithMany(t => t.Students)
            //配置实现多对多关系的联接实体类型。
            .UsingEntity(j =>
                j.ToTable("T_Students_Teachers"));
    }
}
