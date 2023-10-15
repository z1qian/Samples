using Lib;
using Microsoft.EntityFrameworkCore;


Console.WriteLine("请输入您的姓名");
string name = Console.ReadLine() ?? string.Empty;

using TestDBContext ctx = new TestDBContext();
using var tx = await ctx.Database.BeginTransactionAsync();

Console.WriteLine("开启事务成功！准备Select " + DateTime.Now.TimeOfDay);

var h1 = await ctx.Houses.FromSqlInterpolated($"select * from T_Houses with (UPDLOCK) where Id=1").SingleAsync();

Console.WriteLine("返回结果，完成Select " + DateTime.Now.TimeOfDay);

if (string.IsNullOrEmpty(h1.Owner))
{
    Console.WriteLine("准备开始抢购");
    await Task.Delay(5000);
    h1.Owner = name;
    await ctx.SaveChangesAsync();
    Console.WriteLine("抢到手了");
}
else
{
    if (h1.Owner == name)
        Console.WriteLine("这个房子已经是您的了，不用抢");
    else
        Console.WriteLine($"这个房子已经被{h1.Owner}抢走了");
}
await tx.CommitAsync();

Console.ReadLine();