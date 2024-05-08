using AutoMapper;
using FilmsUdemy.Repositories;

namespace FilmsUdemy.Utils.Filters;

public class Filters : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Aqui tendríamos acceso a los parámetros que se pasan al método GetGendersById, es decir a Imppaer, IRespostoryGenderFilm y al id pero solo en el orden en cual aparecen en el método
        // var param1 = (IRespostoryGenderFilm)context.Arguments[0]!;
        // var param2 = (int)context.Arguments[1]!;
        // var param3 = (IMapper)context.Arguments[2]!;
        
        // Aquí ya no sucedería
        var paramRepositoryGender = context.Arguments.OfType<IRespostoryGenderFilm>().FirstOrDefault();
        var paramId = context.Arguments.OfType<int>().FirstOrDefault();
        var paramMapper = context.Arguments.OfType<IMapper>().FirstOrDefault();
        
        // este codigo se ejecutará antes de que se ejecute el método GetGendersById
        var result = await next(context);
 
        // este código se ejecutará después de que se ejecute el método GetGendersById
        return result;
    }
}