namespace _4._6_关系配置;

internal class Order
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    /// <summary>
    /// 快递信息
    /// </summary>
    public Delivery? Delivery { get; set; }
}
