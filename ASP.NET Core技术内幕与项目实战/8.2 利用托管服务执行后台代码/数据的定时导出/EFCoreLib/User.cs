using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace 数据的定时导出;

[Table("T_Users")]
public class User : IdentityUser<long>
{
    public DateTime CreationTime { get; set; }

    public string? NickName { get; set; }
}
