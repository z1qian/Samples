﻿using FluentValidation;
using Zack.DomainCommons.Models;

namespace Listening.Admin.WebAPI.Categories.Request;

//定义只是偶然和CategoryAddRequest一样，所以不应该复用它
public record CategoryUpdateRequest(MultilingualString Name, Uri CoverUrl);

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name.Chinese).NotNull().Length(1, 200);
        RuleFor(x => x.Name.English).NotNull().Length(1, 200);
        RuleFor(x => x.CoverUrl).Length(5, 500);
    }
}