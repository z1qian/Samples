namespace 自动启用事务的操作筛选器;

[AttributeUsage(AttributeTargets.Method)]
public class NotTransactionalAttribute : Attribute
{
}