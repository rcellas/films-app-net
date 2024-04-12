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
        group.MapPost("/", CreateActor).DisableAntiforgery();
        return group;
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