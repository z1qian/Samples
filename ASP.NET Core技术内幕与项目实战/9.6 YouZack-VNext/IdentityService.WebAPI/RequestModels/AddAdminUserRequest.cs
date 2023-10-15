﻿using FluentValidation;

namespace IdentityService.WebAPI.RequestModels;

public record AddAdminUserRequest(string UserName, string PhoneNum);

public class AddAdminUserRequestValidator : AbstractValidator<AddAdminUserRequest>
{
    public AddAdminUserRequestValidator()
    {
        RuleFor(e => e.PhoneNum).NotNull().NotEmpty().Length(11);
        RuleFor(e => e.UserName).NotEmpty().NotEmpty().MaximumLength(20).MinimumLength(2);
    }
}