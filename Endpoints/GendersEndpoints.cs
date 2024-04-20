using System.Runtime.Intrinsics.X86;
using AutoMapper;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
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
        group.MapGet("/", GetAllGenders).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("gender-get"));

        group.MapGet("/${id:int}", GetGendersById);

        // el IOutputCacheStore nos permite borrar la cache de un tag en concreto
        group.MapPost("/", CreateGender);

        group.MapPut("/{id:int}", UpdateGenders);
        group.MapDelete("/{id:int}", DeleteGenders);
        return group;
    }
    // el static hace que el método sea estático y no necesite instanciar la clase para poder llamarlo
// el async hace que el método sea asincrono
// el Task hace que el método devuelva un Task
// el <Ok<List<GenderFilms>>> es el tipo de dato que devolverá el método
static async Task<Ok<List<GenderDTO>>> GetAllGenders(IRespostoryGenderFilm repository, IMapper mapper)
{
    var genders = await repository.GetAll();
    // el TypedResults.Ok nos permite devolver un 200
    // lo que hace el select, que viene de Linq, es que por cada registro que haya en la lista
    // el ToList() lo que hace es que nos devuelva una lista
    var gendersDto = mapper.Map<List<GenderDTO>>(genders);
    return TypedResults.Ok(gendersDto);
}

static async Task<Results<Ok<GenderDTO>,NotFound>> GetGendersById (IRespostoryGenderFilm repository, int id, IMapper mapper)
{
    var gender = await repository.GetById(id);
     
    // si no existe el registro, devolverá un 404
    if (gender is null)
    {
        return TypedResults.NotFound();
    }
    var genderDto = mapper.Map<GenderDTO>(gender);
    // aquí no ponemos solo gender ya que tenemoss que devolver un objeto de tipo Results para que devuelva un 200
    // esto ocurre pq nosotros arriba estamos devolviendo un Results.NotFound() y necesitamos devolver un Results.Ok
    return TypedResults.Ok(genderDto);
}

// el IMapper nos permite mapear un objeto a otro usando AutoMapper
static async Task<Created<GenderDTO>> CreateGender(CreateGenderDTO createGenderDto, IRespostoryGenderFilm repository, IOutputCacheStore outputCacheStore, IMapper mapper)
{
    var gender = mapper.Map<GenderFilms>(createGenderDto);
    var id = await repository.Create(gender);
    // con el await le decimos que espere a que se ejecute el EvictByTagAsync y que al momento de crear el registro, borre la cache
    await outputCacheStore.EvictByTagAsync("gender-get",default);
    
    var genderDto = mapper.Map<GenderDTO>(gender);
    
    return TypedResults.Created($"/gender/{id}", genderDto);
}
// llamamos a CreateGenderDTO pq hace la misma funcionalidad que el create
static async Task<Results<NoContent,NotFound>> UpdateGenders(int id, CreateGenderDTO updateDto, IRespostoryGenderFilm repostory, IOutputCacheStore outputCacheStore, IMapper mapper)
{
    //miramos si existe la id
    var exist = await repostory.Exist(id);

    if (!exist)
    {
        return TypedResults.NotFound();
    }
    
    var gender = mapper.Map<GenderFilms>(updateDto);
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