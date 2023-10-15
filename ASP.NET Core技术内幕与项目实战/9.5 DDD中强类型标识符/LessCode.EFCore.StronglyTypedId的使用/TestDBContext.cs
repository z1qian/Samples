using Microsoft.EntityFrameworkCore;

namespace LessCode.EFCore.StronglyTypedId的使用;

internal class TestDBContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLExpress;DataBase=DemoDB;Trusted_Connection=True;trustServerCertificate=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureStronglyTypedId();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.ConfigureStronglyTypedIdConventions(this);
    }
}
