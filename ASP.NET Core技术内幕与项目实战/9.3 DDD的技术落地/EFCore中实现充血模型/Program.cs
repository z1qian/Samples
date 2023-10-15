using EFCoreLib;
using EFCore中实现充血模型;

using TestDBContext ctx = new TestDBContext();

//User u1 = new User("Zack");
//u1.Tag = "MyTag";
//u1.ChangePassword("123456");

//ctx.Users.Add(u1);
//ctx.SaveChanges();


User u1 = ctx.Users.First(u => u.UserName == "Zack");
Console.WriteLine(u1);