namespace Users.WebAPI;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,
    AllowMultiple = false, Inherited = true)]
 public class UnitOfWorkAttribute : Attribute
 {
    public Type[] DbContextTypes { get; init; }
    public UnitOfWorkAttribute(params Type[] dbContextTypes)
    {
       this.DbContextTypes = dbContextTypes;
   }
 }