using LessCode.EFCore.StronglyTypedId的使用;
using StronglyTypedId的使用;

using TestDBContext ctx = new TestDBContext();

Person p1 = new Person()
{
    Name = "yzk"
};

ctx.Persons.Add(p1);
ctx.SaveChanges();

PersonId id = p1.Id;
Console.WriteLine(id);

Person p = FindById(new PersonId(1))!;
Console.WriteLine(p.Name);

Person? FindById(PersonId pid)
{
    return ctx.Persons.SingleOrDefault(p => p.Id == pid);
}