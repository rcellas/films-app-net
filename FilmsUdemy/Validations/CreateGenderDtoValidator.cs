using FilmsUdemy.DTOs;
using FluentValidation;

namespace FilmsUdemy.Validations;

//AbstrascValidator es una clase de FluentValidation que nos permite validar los DTOs atraves de reglas y de la abstracción de la clase
public class CreateGenderDtoValidator : AbstractValidator<CreateGenderDTO>
{
    public CreateGenderDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}