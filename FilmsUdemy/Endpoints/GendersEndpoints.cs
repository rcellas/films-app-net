using AutoMapper;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
using FilmsUdemy.Utils.Filters;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace FilmsUdemy.Endpoints;

// lo ponemos como static para que no se pueda instanciar, es decir que no se pueda crear un objeto de esta clase
public static class GendersEndpoints
{
    // el RouteGroupBuilder nos permite crear rutas, esto viene de la librería de Microsoft.AspNetCore.Routing
    // el RouterGroupBuilder viene de MapGroup, que es un método que nos permite crear rutas
    public static RouteGroupBuilder MapGenders(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllGenders).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("gender-get")).RequireAuthorization();

        // el  context hace referencia a la petición que se ha hecho
        // el next es un delegado que nos permite ejecutar el siguiente middleware, esto quiere decir que si no se ejecuta el next, no se ejecutará el método GetGendersById
        group.MapGet("/${id:int}", GetGendersById).AddEndpointFilter<Filters>();
        // el IOutputCacheStore nos permite borrar la cache de un tag en concreto
        group.MapPost("/", CreateGender).AddEndpointFilter<FilterValidation<CreateGenderDTO>>();

        group.MapPut("/{id:int}", UpdateGenders).AddEndpointFilter<FilterValidation<CreateGenderDTO>>();
        group.MapDelete("/{id:int}", DeleteGenders);
        return group;
    }
    // el static hace que el método sea estático y no necesite instanciar la clase para poder llamarlo
    // el async hace que el método sea asincrono
    // el Task hace que el método devuelva un Task
    // el <Ok<List<GenderFilms>>> es el tipo de dato que devolverá el método
    static async Task<Ok<List<GenderDto>>> GetAllGenders(IRespostoryGenderFilm repository, IMapper mapper)
    {
        var genders = await repository.GetAll();
        // el TypedResults.Ok nos permite devolver un 200
        // lo que hace el select, que viene de Linq, es que por cada registro que haya en la lista
        // el ToList() lo que hace es que nos devuelva una lista
        var gendersDto = mapper.Map<List<GenderDto>>(genders);
        return TypedResults.Ok(gendersDto);
    }

    static async Task<Results<Ok<GenderDto>,NotFound>> GetGendersById (IRespostoryGenderFilm repository, int id, IMapper mapper)
    {
        var gender = await repository.GetById(id);
         
        // si no existe el registro, devolverá un 404
        if (gender is null)
        {
            return TypedResults.NotFound();
        }
        var genderDto = mapper.Map<GenderDto>(gender);
        // aquí no ponemos solo gender ya que tenemoss que devolver un objeto de tipo Results para que devuelva un 200
        // esto ocurre pq nosotros arriba estamos devolviendo un Results.NotFound() y necesitamos devolver un Results.Ok
        return TypedResults.Ok(genderDto);
    }

    // el IMapper nos permite mapear un objeto a otro usando AutoMapper
    static async Task<Results<Created<GenderDto>, ValidationProblem>> CreateGender(CreateGenderDTO createGenderDto, IRespostoryGenderFilm repository, IOutputCacheStore outputCacheStore, IMapper mapper)
    {
       
        var gender = mapper.Map<Gender>(createGenderDto);
        var id = await repository.Create(gender);
        // con el await le decimos que espere a que se ejecute el EvictByTagAsync y que al momento de crear el registro, borre la cache
        await outputCacheStore.EvictByTagAsync("gender-get",default);
        
        var genderDto = mapper.Map<GenderDto>(gender);
        
        return TypedResults.Created($"/gender/{id}", genderDto);
    }
    // llamamos a CreateGenderDTO pq hace la misma funcionalidad que el create
    static async Task<Results<NoContent,NotFound, ValidationProblem>> UpdateGenders(int id, CreateGenderDTO updateDto, IRespostoryGenderFilm repostory, IOutputCacheStore outputCacheStore, IMapper mapper )
    {
        
        
        //miramos si existe la id
        var exist = await repostory.Exist(id);

        if (!exist)
        {
            return TypedResults.NotFound();
        }
        
        var gender = mapper.Map<Gender>(updateDto);
        gender.Id = id;
        
        await repostory.Update(gender);
        await outputCacheStore.EvictByTagAsync("gender-get", default);
            
        // con el Results.NoContent() le decimos que no devuelva nada, pero que el registro se ha actualizado correctamente
        return TypedResults.NoContent();
    }

    static async Task<Results<NoContent, NotFound>> DeleteGenders(int id, IRespostoryGenderFilm repository,
        IOutputCacheStore outputCacheStore)
    {
        var exist = await repository.Exist(id);

        if (!exist)
        {
            return TypedResults.NotFound();
        }

        await repository.Delete(id);
        await outputCacheStore.EvictByTagAsync("gender-get", default);
        return TypedResults.NoContent();
        }
}