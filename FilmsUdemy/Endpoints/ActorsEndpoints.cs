using AutoMapper;
using FilmsUdemy.DTOs.Actors;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories.Actors;
using FilmsUdemy.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace FilmsUdemy.Endpoints;

public static class ActorsEndpoints
{
    private static readonly string container="actors";
    public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllActors).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("actors-get"));
        group.MapGet("/{id}", GetActorById);
        group.MapPost("/", CreateActor).DisableAntiforgery();
        return group;
    }
    
    static async Task<Ok<List<ActorsDTO>>> GetAllActors(IRepositoryActors repositoryActors, IMapper mapper)
    {
        var actors = await repositoryActors.GetAllActors();
        var actorsDtos = mapper.Map<List<ActorsDTO>>(actors);
        return TypedResults.Ok(actorsDtos);
    }
    
    static async Task<Results<Ok<ActorsDTO>, NotFound>> GetActorById(int id, IRepositoryActors repositoryActors, IMapper mapper)
    {
        var actor = await repositoryActors.GetActorById(id);
        if (actor is null)
        {
            return TypedResults.NotFound();
        }
        var actorDto = mapper.Map<ActorsDTO>(actor);
        return TypedResults.Ok(actorDto);
    }
    
    // fromform nos sirve para recibir archivos desde el cliente
    static async Task<Created<ActorsDTO>> CreateActor([FromForm] CreateActorsDTO createActorsDto,
        IRepositoryActors repositoryActors, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
    {
        var actor = mapper.Map<Actor>(createActorsDto);
        if (createActorsDto.Photo is not null)
        {
            string url = await fileStorage.Storage(container, createActorsDto.Photo);
            actor.Photo = url;
        }
        int id = await repositoryActors.CreateActor(actor);
        await outputCacheStore.EvictByTagAsync("actors-get", default);
        var actorDTO = mapper.Map<ActorsDTO>(actor);
        return TypedResults.Created($"/actors/{id}", actorDTO);

    }
    
    
}