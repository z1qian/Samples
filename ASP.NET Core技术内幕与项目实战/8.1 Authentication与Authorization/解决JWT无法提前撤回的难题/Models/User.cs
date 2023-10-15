using Microsoft.AspNetCore.Identity;

namespace 解决JWT无法提前撤回的难题;

public class User : IdentityUser<long>
{
    public DateTime CreationTime { get; set; }

    public string? NickName { get; set; }

    public long JWTVersion { get; set; }
}