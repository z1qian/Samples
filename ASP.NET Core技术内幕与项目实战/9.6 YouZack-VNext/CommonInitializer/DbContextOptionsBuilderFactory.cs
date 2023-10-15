using Microsoft.EntityFrameworkCore;

namespace CommonInitializer;

public class DbContextOptionsBuilderFactory
{
    public static DbContextOptionsBuilder<TDBContext> Create<TDBContext>() where TDBContext : DbContext
    {
        string? connStr = Environment.GetEnvironmentVariable("DefaultDB:ConnStr");
        ThrowHelper.ThorwArgumentNullExceptionIfNullOrWhiteSpace(connStr, "DefaultDB:ConnStr", "未设置环境变量项：DefaultDB:ConnStr");

        var optionsBuilder = new DbContextOptionsBuilder<TDBContext>();
        optionsBuilder.UseSqlServer(connStr);

        return optionsBuilder;
    }
}
