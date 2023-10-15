namespace Lib;
public class House
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Owner { get; set; }

    //并发令牌属性列
    public byte[] RowVer { get; set; }
}
