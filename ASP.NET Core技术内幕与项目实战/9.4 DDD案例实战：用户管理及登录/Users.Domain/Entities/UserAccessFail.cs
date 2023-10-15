namespace Users.Domain.Entities;

public record UserAccessFail
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }                      //用户ID
    public User User { get; init; }                        //用户

    private bool lockOut;                                  //是否锁定
    public DateTime? LockoutEnd { get; private set; }      //锁定结束期
    public int AccessFailedCount { get; private set; }     //登录失败次数
    private UserAccessFail() { }
    public UserAccessFail(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }
    public void Reset()                   //重置登录失败信息
    {
        lockOut = false;
        LockoutEnd = null;
        AccessFailedCount = 0;
    }
    public bool IsLockOut()               //是否已经锁定
    {
        if (lockOut)
        {
            if (LockoutEnd >= DateTime.Now)
            {
                return true;
            }
            else                            //锁定已经到期
            {
                AccessFailedCount = 0;
                LockoutEnd = null;
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void Fail()                      //处理一次“登录失败”
    {
        AccessFailedCount++;
        if (AccessFailedCount >= 3)
        {
            lockOut = true;
            LockoutEnd = DateTime.Now.AddMinutes(5);
        }
    }
}