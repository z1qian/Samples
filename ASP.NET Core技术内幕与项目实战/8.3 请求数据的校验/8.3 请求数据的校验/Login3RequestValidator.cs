using FluentValidation;

namespace _8._3_请求数据的校验;

public class Login3RequestValidator : AbstractValidator<Login3Request>
{
    public Login3RequestValidator(/*通过依赖注入的方式进行服务的注入*/TestDbContext dbCtx)
    {
        //RuleFor(x => x.UserName).NotNull()
        //   .Must(name => dbCtx.Users.Any(u => u.UserName == name))
        //   .WithMessage(c => $"用户名{c.UserName}不存在");

        //异步校验规则
        RuleFor(x => x.UserName).NotNull()
            .MustAsync((name, _) => dbCtx.Users.AnyAsync(u => u.UserName == name))
            .WithMessage(c => $"用户名{c.UserName}不存在");
    }
}