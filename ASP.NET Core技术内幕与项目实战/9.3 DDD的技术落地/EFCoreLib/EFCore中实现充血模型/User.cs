/*
 * 1.属性是只读的或者只能被类内部的代码修改
 * 
 * 2.定义了有参构造方法
 * 
 * 3.有的成员变量没有对应属性，但是这些成员变量需要映射为数据库表中的列，也就是我们需要把私有成员变量映射到数据库表中的列
 * 
 * 4.有的属性是只读的，也就是它的值是从数据库中读取出来的，但是我们不能修改属性的值
 * 
 * 5.有的属性不需要映射到数据列，仅在运行时被使用
 */

namespace EFCore中实现充血模型;
public record User
{
    public long Id { get; init; }                    //特征一
    public DateTime CreatedDateTime { get; init; }  //特征一
    public string UserName { get; private set; }    //特征一
    public int Credit { get; private set; }         //特征一

    private string? passwordHash;                   //特征三




    private string? remark /*= "杨中科先生"*/;
    public string? Remark                           //特征四
    {
        get
        {
            Console.WriteLine($"\n{nameof(Remark)}属性的get代码块被调用");
            return remark ?? "null";
        }
    }



    public string? Tag { get; set; }                //特征五

    //EFCore创建对象时通过反射使用
    private User()                                  //特征二
    {
    }

    //开发人员创建对象时使用
    public User(string yhm)                         //特征二
    {
        this.UserName = yhm;
        this.CreatedDateTime = DateTime.Now;
        this.Credit = 10;
    }
    public void ChangeUserName(string newValue)
    {
        this.UserName = newValue;
    }
    public void ChangePassword(string newValue)
    {
        if (newValue.Length < 6)
        {
            throw new ArgumentException("密码太短");
        }
        this.passwordHash = $"这是被转换成hash值的:{newValue}";
    }
}