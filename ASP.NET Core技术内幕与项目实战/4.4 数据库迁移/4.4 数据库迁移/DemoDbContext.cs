using Microsoft.EntityFrameworkCore;

namespace _4._4_数据库迁移;

public partial class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TAuthor> TAuthors { get; set; }

    public virtual DbSet<TBook> TBooks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=DemoDB;Trusted_Connection=True;trustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAuthor>(entity =>
        {
            entity.ToTable("T_Authors");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("(N'')");
        });

        modelBuilder.Entity<TBook>(entity =>
        {
            entity.ToTable("T_Books");

            entity.Property(e => e.AuthorName).HasMaxLength(20);
            entity.Property(e => e.Remark)
                .HasMaxLength(100)
                .HasDefaultValueSql("(N'暂无备注')");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
