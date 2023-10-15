using StronglyTypedId的使用;

namespace LessCode.EFCore.StronglyTypedId的使用;

[HasStronglyTypedId]
internal class Person
{
    public PersonId Id { get; set; }

    public string Name { get; set; }
}
