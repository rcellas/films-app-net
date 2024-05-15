using FluentValidation;

namespace FilmsUdemy.Utils.Filters;

public class FilterValidation<T>: IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validate = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validate is null)
        {
            return await next(context);
        }

        var insumoToValidate = context.Arguments.OfType<T>().FirstOrDefault();

        if (insumoToValidate is null)
        {
            return TypedResults.Problem("No se encontró la entidad a validar");
        }
        
        var resultValidation = await validate.ValidateAsync(insumoToValidate);

        if (!resultValidation.IsValid)
        {
            return TypedResults.ValidationProblem(resultValidation.ToDictionary());
        }
        return await next(context);
    }
}