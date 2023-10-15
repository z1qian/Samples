using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _4._6_关系配置
{
    internal class LeaveConfig : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.ToTable("T_Leaves");

            builder.HasOne(l => l.Requester)
                .WithMany()
                .IsRequired();

            builder.HasOne(l => l.Approver)
                .WithMany();

            builder.Property(l => l.Remarks)
                .HasMaxLength(1000)
                .IsUnicode();
        }
    }
}
