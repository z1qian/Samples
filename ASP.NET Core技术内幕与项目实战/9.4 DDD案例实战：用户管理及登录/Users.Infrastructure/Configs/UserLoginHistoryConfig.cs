using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Configs
{
    class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("T_UserLoginHistories");
            builder.OwnsOne(x => x.PhoneNumber, nb =>
            {
                nb.Property(x => x.RegionCode);
                nb.Property(x => x.Number).HasMaxLength(20).IsUnicode(true);
            });
        }
    }
}
