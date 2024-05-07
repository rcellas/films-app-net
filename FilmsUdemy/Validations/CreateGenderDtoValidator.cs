using FilmsUdemy.DTOs;
using FluentValidation;

namespace FilmsUdemy.Validations;

//AbstrascValidator es una clase de FluentValidation que nos permite validar los DTOs atraves de reglas y de la abstracción de la clase
public class CreateGenderDtoValidator : AbstractValidator<CreateGenderDTO>
{
    public CreateGenderDtoValidator()
    {
        //mostrar mensaje de error no personalizado
        //RuleFor(x => x.Name).NotEmpty();
        
        //mostrar mensaje de error personalizado
        RuleFor(x => x.Name).NotEmpty().WithMessage("El campo nombre es requerido").MaximumLength(50).WithMessage("El campo nombre no debe tener más de {MaxLength} caracteres");
    }
}