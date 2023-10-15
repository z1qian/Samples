namespace _4._6_关系配置;

internal class Teacher
{
    public Teacher()
    {
        Students = new List<Student>();
    }
    public long Id { get; set; }
    public string? Name { get; set; }
    public List<Student> Students { get; set; }
}
