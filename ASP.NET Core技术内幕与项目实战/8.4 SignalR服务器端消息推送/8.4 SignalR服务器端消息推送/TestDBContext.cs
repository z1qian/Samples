using Microsoft.EntityFrameworkCore;

namespace _8._4_SignalR服务器端消息推送;

public class TestDBContext : DbContext
{
    public TestDBContext(DbContextOptions<TestDBContext> options)
       : base(options)
    {
    }
}