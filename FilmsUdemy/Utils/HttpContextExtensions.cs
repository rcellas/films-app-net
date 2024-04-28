using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Utils;

// HttpContext es una clase que nos permite acceder a la informacion de la peticion HTTP
public static class HttpContextExtension
{
    // esto es una extension de la clase HttpContext que nos permite insertar parametros de paginacion en el header
    public async static Task InsertParametersPaginationInHeader<T>(this HttpContext httpContext,
        IQueryable<T> queryable)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }
        
        // double es un tipo de dato que nos permite guardar numeros con decimales
        // nos permite hacer una consulta a la base de datos sin traer los datos, solo el conteo
        // CountAsync nos permite hacer una consulta asincrona a la base de datos y nos devolverá el conteo de los registros
        double count = await queryable.CountAsync();
        httpContext.Response.Headers.Append("totalAmountOfRecords", count.ToString());
    }
}