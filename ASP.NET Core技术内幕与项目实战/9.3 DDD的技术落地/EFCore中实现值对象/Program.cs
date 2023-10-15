using EFCoreLib;
using EFCore中实现值对象;

using TestDBContext ctx = new TestDBContext();

//MultilingualString name1 = new MultilingualString("北京", "BeiJing");

//Area area1 = new Area(16410, AreaType.SquareKM);

//Geo loc = new Geo(116.4074, 39.9042);

//Region c1 = new Region(name1, area1, loc, RegionLevel.Province);

//c1.ChangePopulation(21893100);

//ctx.Region.Add(c1);
//ctx.SaveChanges();

ctx.Region.Where(简化值对象的比较帮助类.MakeEqual((Region c) => c.Name,
    new MultilingualString("北京", "BeiJing")));