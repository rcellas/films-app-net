using FilmsUdemy.DTOs.Comment;
using FilmsUdemy.Validations.Utils;
using FluentValidation;

namespace FilmsUdemy.Utils.Filters;

public class CommentsValidator:AbstractValidator<CreateCommentsDto>
{
    public CommentsValidator()
    {
        RuleFor(x=> x.Body).NotEmpty().WithMessage(MessageValidationsFrequents.MessageRequired);   
    }
}