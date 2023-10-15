using FluentValidation;

namespace _8._3_请求数据的校验;

public class Login2RequestValidator : AbstractValidator<Login2Request>
{
    public Login2RequestValidator()
    {
        RuleFor(x => x.Email).NotNull().EmailAddress()
            .WithMessage("邮箱格式不正确")
            .Must(v => v.EndsWith("@qq.com") || v.EndsWith("@163.com"))
            //WithMessage方法设置的报错信息只作用于它之前的那个校验规则
            .WithMessage("只支持QQ和163邮箱");

        RuleFor(x => x.Password).NotNull().Length(3, 10)
            .WithMessage("密码长度必须介于3到10之间")
            .Equal(x => x.Password2).WithMessage("两次密码必须一致");
    }
}