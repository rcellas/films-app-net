using FilmsUdemy.DTOs.Films;
using FluentValidation;

namespace FilmsUdemy.Validations.Utils;

public class CreateFilmsValidators:AbstractValidator<CreateFilmsDto>
{
    public CreateFilmsValidators()
    {
        RuleFor(x=> x.Title).NotEmpty().WithMessage(MessageValidationsFrequents.MessageRequired).MaximumLength(100).WithMessage(MessageValidationsFrequents.MessageMaxLength);
    }
}