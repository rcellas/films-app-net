using FilmsUdemy.DTOs;
using FilmsUdemy.Repositories;
using FilmsUdemy.Validations.Utils;
using FluentValidation;

namespace FilmsUdemy.Validations;

//AbstrascValidator es una clase de FluentValidation que nos permite validar los DTOs atraves de reglas y de la abstracción de la clase
public class CreateGenderDtoValidator : AbstractValidator<CreateGenderDTO>
{
    public CreateGenderDtoValidator(IRespostoryGenderFilm respostoryGender, IHttpContextAccessor httpContextAccessor)
    {
        // esto nos permite obtener el id de la ruta que se ha pasado por parámetro y de esta forma poder hacer validaciones para que se pueda cambiar el nombre del género
        var valueRouteId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
        var id =0;
        if(valueRouteId is string idString)
        {
            int.TryParse(idString, out id);
        }
        
        //mostrar mensaje de error no personalizado
        //RuleFor(x => x.Name).NotEmpty();
        
        //mostrar mensaje de error personalizado
        RuleFor(x => x.Name).NotEmpty().WithMessage(MessageValidationsFrequents.MessageRequired).MaximumLength(50).WithMessage(MessageValidationsFrequents.MessageMaxLength).Must(FirstLetterMayus).WithMessage(MessageValidationsFrequents.MessageMayu).MustAsync(async (name, _) =>
        {
            var exits = await respostoryGender.SameName(id:0, name);
            return !exits;
        }).WithMessage(MessageValidationsFrequents.MessageExist);
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