using Microsoft.AspNetCore.Identity;

namespace 标识框架的使用.Models;

public class User : IdentityUser<long>
{
    public DateTime CreationTime { get; set; }

    public string? NickName { get; set; }
}
