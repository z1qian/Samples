using BooksEFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly MyDbContext dbCtx;
    public TestController(MyDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var book = await dbCtx.Books.FirstAsync();
        return Content(book.ToString());
    }
}