using FilmsUdemy.DTOs;
using FilmsUdemy.Repositories;
using FluentValidation;

namespace FilmsUdemy.Validations;

//AbstrascValidator es una clase de FluentValidation que nos permite validar los DTOs atraves de reglas y de la abstracción de la clase
public class CreateGenderDtoValidator : AbstractValidator<CreateGenderDTO>
{
    public CreateGenderDtoValidator(IRespostoryGenderFilm respostoryGender)
    {
        //mostrar mensaje de error no personalizado
        //RuleFor(x => x.Name).NotEmpty();
        
        //mostrar mensaje de error personalizado
        RuleFor(x => x.Name).NotEmpty().WithMessage("El campo nombre es requerido").MaximumLength(50).WithMessage("El campo nombre no debe tener más de {MaxLength} caracteres").Must(FirstLetterMayus).WithMessage("El campo nombre debe empezar con mayúscula").MustAsync(async (name, _) =>
        {
            var exits = await respostoryGender.SameName(id:0, name);
            return !exits;
        }).WithMessage("El campo nombre no se puede crear porque ya existe en la base de datos");
    }
    // validación sincrona
    private bool FirstLetterMayus(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        string firstLetter = name[0].ToString();
        return firstLetter == firstLetter.ToUpper();
    }
    
}