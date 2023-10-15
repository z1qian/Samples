using FluentValidation;

namespace FileService.WebAPI.RequestModels;

public record UploadRequest(IFormFile File);

public class UploadRequestValidator : AbstractValidator<UploadRequest>
{
    public UploadRequestValidator()
    {
        //最大文件大小
        long maxFileSize = 50 * 1024 * 1024;

        RuleFor(r => r.File)
            .NotNull()
            .Must(f => f.Length > 0 && f.Length <= maxFileSize)
            .WithMessage("音频最大长度为50MB");
    }
}