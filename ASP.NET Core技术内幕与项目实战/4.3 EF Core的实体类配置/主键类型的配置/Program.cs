using 主键类型的配置;

using TestDBContext ctx = new TestDBContext();

Console.WriteLine("****1*****");
Author a1 = new Author { Name = "杨中科" };
//Id=00000000-0000-0000-0000-000000000000
Console.WriteLine($"Add前，Id={a1.Id}");
ctx.Authors.Add(a1);
//Id=b809062c-7c76-4046-8d1f-08db714b4c88
Console.WriteLine($"Add后，保存前，Id={a1.Id}");
await ctx.SaveChangesAsync();
//Id=b809062c-7c76-4046-8d1f-08db714b4c88
Console.WriteLine($"保存后，Id={a1.Id}");

Console.WriteLine("****2*****");
Author a2 = new Author { Name = "Zack Yang" };
a2.Id = Guid.NewGuid();
//Id=9aef1896-67d8-4927-87b6-130ff8b29ce3
Console.WriteLine($"保存前，Id={a2.Id}");
ctx.Authors.Add(a2);
await ctx.SaveChangesAsync();
//Id=9aef1896-67d8-4927-87b6-130ff8b29ce3
Console.WriteLine($"保存前，Id={a2.Id}");