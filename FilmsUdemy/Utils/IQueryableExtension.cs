using FilmsUdemy.DTOs;
using FilmsUdemy.DTOs.Actors;

namespace FilmsUdemy.Utils;

public static class IQueryableExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto pagination)
    {
        // Skip nos permite saltar los primeros registros, con este metodo conseguimos que si queremos saltarnos por ejemplo los 10 primeros registros, le decimos que salte los 10 primeros registros y ya iria a la pagina 2
        // pagination.Page - 1 nos permite restarle 1 a la pagina para que empiece desde 0
        // pagination.RecordsForPage nos permite decirle cuantos registros queremos que nos devuelva por pagina
        // Take nos permite decirle cuantos registros queremos que nos devuelva
        return queryable.Skip((pagination.Page - 1) * pagination.RecordsForPage).Take(pagination.RecordsForPage);
    }
}