using EFCoreLib;
using System.Linq.Expressions;

var items = Query<Book>("Id", "PubTime", "Title");
foreach (object[] row in items)
{
    long id = (long)row[0];
    DateTime pubTime = (DateTime)row[1];
    string title = (string)row[2];

    Console.WriteLine(id + "," + pubTime + "," + title);
}

IEnumerable<object[]> Query<TEntity>(params string[] propNames) where TEntity : class
{
    ParameterExpression exParameter = Expression.Parameter(typeof(TEntity),"t");

    List<Expression> exProps = new List<Expression>();
    foreach (var name in propNames)
    {
        Expression exProp = Expression.Convert(Expression.MakeMemberAccess(exParameter, typeof(TEntity).GetProperty(name)),
            typeof(object));

        exProps.Add(exProp);
    }

    NewArrayExpression newArrayExp = Expression.NewArrayInit(typeof(object), exProps);

    var selectExpression = Expression.Lambda<Func<TEntity, object[]>>(newArrayExp, exParameter);
    Console.WriteLine(selectExpression);

    using TestDBContext ctx = new TestDBContext();

    IQueryable<object[]> selectQueryable = ctx.Set<TEntity>().Select(selectExpression);

    return selectQueryable.ToArray();
}