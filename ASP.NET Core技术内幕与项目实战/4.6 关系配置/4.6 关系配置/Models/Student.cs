namespace _4._6_关系配置;

internal class Student
{
    public Student()
    {
        Teachers = new List<Teacher>();
    }

    public long Id { get; set; }
    public string? Name { get; set; }
    public List<Teacher> Teachers { get; set; }
}
