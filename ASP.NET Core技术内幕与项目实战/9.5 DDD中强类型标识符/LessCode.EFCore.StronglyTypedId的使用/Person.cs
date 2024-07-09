using StronglyTypedId的使用;

namespace LessCode.EFCore.StronglyTypedId的使用;

[HasStronglyTypedId(typeof(Guid))]
internal class Person
{
    public PersonId Id { get; protected set; }

    public string Name { get; set; }
}
