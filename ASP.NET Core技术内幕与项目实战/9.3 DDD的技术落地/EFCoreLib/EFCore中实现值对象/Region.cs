namespace EFCore中实现值对象;

public record Region
{
    public long Id { get; init; }

    /// <summary>
    /// 名字
    /// </summary>
    public MultilingualString Name { get; init; }

    /// <summary>
    /// 面积
    /// </summary>
    public Area Area { get; init; }

    /// <summary>
    /// 级别
    /// </summary>
    public RegionLevel Level { get; private set; }

    /// <summary>
    /// 总人口
    /// </summary>
    public long? Population { get; private set; }

    /// <summary>
    /// 地理位置
    /// </summary>
    public Geo Location { get; init; }

    private Region() { }
    public Region(MultilingualString name, Area area, Geo location,
       RegionLevel level)
    {
        this.Name = name;
        this.Area = area;
        this.Location = location;
        this.Level = level;
    }
    public void ChangePopulation(long value)
    {
        this.Population = value;
    }
    public void ChangeLevel(RegionLevel value)
    {
        this.Level = value;
    }
}

public record MultilingualString(string Chinese, string? English);
public record Area(double Value, AreaType Unit);
public enum AreaType
{
    /// <summary>
    /// 平方公里
    /// </summary>
    SquareKM,
    /// <summary>
    /// 公顷
    /// </summary>
    Hectare,
    /// <summary>
    /// 市亩
    /// </summary>
    CnMu
};
public enum RegionLevel
{
    /// <summary>
    /// 省
    /// </summary>
    Province,
    /// <summary>
    /// 市
    /// </summary>
    City,
    /// <summary>
    /// 县
    /// </summary>
    County,
    /// <summary>
    /// 镇
    /// </summary>
    Town
};
public record Geo
{
    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; init; }

    public Geo(double longitude, double latitude)
    {
        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentException("longitude invalid");
        }
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentException("longitude invalid");
        }
        this.Longitude = longitude;
        this.Latitude = latitude;
    }
}