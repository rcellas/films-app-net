using System.Data;
using FilmsUdemy.DTOs.Actors;
using FilmsUdemy.Validations.Utils;
using FluentValidation;

namespace FilmsUdemy.Validations;

public class CreateActorDtoValidator : AbstractValidator<CreateActorsDTO>
{
    public CreateActorDtoValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty()
            .WithMessage(MessageValidationsFrequents.MessageRequired)
            .MaximumLength(150)
            .WithMessage("The name can't have more than 50 characters");

        var dateMinimal = new DateTime(1900, 1, 1);
        
        RuleFor(x=>x.DateOfBirth).GreaterThanOrEqualTo(dateMinimal).WithMessage("El actor debe haber nacido después de "+dateMinimal.ToString("yyyy-M-d dddd"));
    }
    
}