namespace Users.Domain.Entities;

public record User : IAggregateRoot
{
    public Guid Id { get; init; }
    public PhoneNumber PhoneNumber { get; private set; }//手机号

    private string? passwordHash;                       //密码的哈希值
    public UserAccessFail AccessFail { get; private set; }
    private User() { }                                    //供EF Core加载数据使用
    public User(PhoneNumber phoneNumber)
    {
        Id = Guid.NewGuid();
        PhoneNumber = phoneNumber;
        AccessFail = new UserAccessFail(this);
    }
    //实体类中定义的方法只是和特定实体类相关的业务逻辑代码
    public bool HasPassword()                              //是否设置了密码
    {
        return !string.IsNullOrEmpty(passwordHash);
    }
    public void ChangePassword(string value)               //修改密码
    {
        if (value.Length <= 3)
            throw new ArgumentException("密码长度不能小于3");
        passwordHash = HashHelper.ComputeMd5Hash(value);
    }
    public bool CheckPassword(string password)             //检查密码是否正确
    {
        return passwordHash == HashHelper.ComputeMd5Hash(password);
    }
    public void ChangePhoneNumber(PhoneNumber phoneNumber) //修改手机号
    {
        PhoneNumber = phoneNumber;
    }
}