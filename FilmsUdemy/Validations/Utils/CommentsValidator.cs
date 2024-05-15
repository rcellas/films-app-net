using FilmsUdemy.DTOs.Comment;
using FluentValidation;

namespace FilmsUdemy.Validations.Utils;

public abstract class CommentsValidator:AbstractValidator<CreateCommentsDto>
{
    public CommentsValidator()
    {
        RuleFor(x=> x.Body).NotEmpty().WithMessage(MessageValidationsFrequents.MessageRequired);   
    }
}