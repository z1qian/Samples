using EFCoreLib;
using Microsoft.AspNetCore.Mvc;

namespace 自动启用事务的操作筛选器.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpPost]
    //[NotTransactional]
    public async Task Save()
    {
        using TestDBContext dbCtx = new TestDBContext();

        dbCtx.Books.Add(new Book { Title = "测试图书qwe", PubTime = DateTime.Now, Price = 25.12, AuthorName = "zzx", Remark = "自动提交事务" });
        await dbCtx.SaveChangesAsync();

        dbCtx.Books.Add(new Book { Title = "测试图书asd", PubTime = null, Price = 33.22,  Remark = "自动提交事务" });
        await dbCtx.SaveChangesAsync();
    }
}
