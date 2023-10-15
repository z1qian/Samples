using Microsoft.AspNetCore.Mvc;

namespace EFCore中发布领域事件的合适时机.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test1Controller : ControllerBase
{
    private UserDbContext context;

    public Test1Controller(UserDbContext context)
    {
        this.context = context;
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest req)
    {
        User? user = context.Users.Find(id);
        if (user == null)
        {
            return NotFound($"id={id}的User不存在");
        }
        user.ChangeAge(req.Age);
        user.ChangeEmail(req.Email);
        user.ChangeNickName(req.NickName);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddUserRequest req)
    {
        var user = new User(req.UserName, req.Email);
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    [Route("id")]
    public async Task<IActionResult> Delete(Guid id)
    {
        User? user = context.Users.Find(id);
        if (user == null)
        {
            return NotFound($"id={id}的User不存在");
        }
        user.SoftDelete();
        await context.SaveChangesAsync();
        return Ok();
    }
}
