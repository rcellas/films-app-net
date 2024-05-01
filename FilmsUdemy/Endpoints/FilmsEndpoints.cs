using AutoMapper;
using FilmsUdemy.DTOs;
using FilmsUdemy.DTOs.Films;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories.Films;
using FilmsUdemy.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace FilmsUdemy.Endpoints;

public static class FilmsEndpoints
{
    public static readonly string _container = "films";
public static RouteGroupBuilder MapFilms(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllFilms).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("films-get"));
        group.MapGet("getByName/{name}", GetFilmsByName);
        group.MapGet("/{id}", GetFilmById);
        group.MapPost("/", CreateFilm).DisableAntiforgery();
        group.MapPut("/{id:int}", UpdateFilm).DisableAntiforgery();
        group.MapDelete("/{id:int}", DeleteFilm);
        return group;
    }

    static async Task<Ok<List<FilmsDto>>> GetAllFilms(IRepositoryFilms repositoryFilms, IMapper mapper, int page = 1, int recordsForPage = 10)
    {
        var pagination = new PaginationDto() { Page = page, RecordsForPage = recordsForPage };
        var films = await repositoryFilms.GetAllFilms(pagination);
        var filmsDtos = mapper.Map<List<FilmsDto>>(films);
        return TypedResults.Ok(filmsDtos);
    }
    
    static async Task<Ok<List<FilmsDto>>> GetFilmsByName(string name, IRepositoryFilms repositoryFilms, IMapper mapper)
    {
        var films = await repositoryFilms.GetFilmsByName(name);
        var filmsDtos = mapper.Map<List<FilmsDto>>(films);
        return TypedResults.Ok(filmsDtos);
    }
    
    static async Task<Results<Ok<FilmsDto>, NotFound>> GetFilmById(int id, IRepositoryFilms repositoryFilms, IMapper mapper)
    {
        var film = await repositoryFilms.GetFilmById(id);
        if (film is null)
        {
            return TypedResults.NotFound();
        }
        var filmDto = mapper.Map<FilmsDto>(film);
        return TypedResults.Ok(filmDto);
    }
    
    static async Task<Created<FilmsDto>> CreateFilm([FromForm] CreateFilmsDto createFilmsDto,
        IRepositoryFilms repositoryFilms, IOutputCacheStore outputCacheStore, IMapper mapper, IFileStorage fileStorage)
    {
        var film = mapper.Map<Film>(createFilmsDto);
        if (createFilmsDto.Poster is not null)
        {
            string url = await fileStorage.Storage(_container, createFilmsDto.Poster);
            film.Poster = url;
        }
        var id = await repositoryFilms.CreateFilm(film);
        await outputCacheStore.EvictByTagAsync("films-get", default);
        var filmDto = mapper.Map<FilmsDto>(film);
        return TypedResults.Created($"/films/{id}", filmDto);
    }
    
    static async Task<Results<NoContent, NotFound>> UpdateFilm(int id, [FromForm] CreateFilmsDto updateFilmsDto, IRepositoryFilms repositoryFilms, IMapper mapper, IFileStorage fileStorage, IOutputCacheStore outputCacheStore)
    {
        var filmDb = await repositoryFilms.GetFilmById(id);
        if (filmDb is null)
        {
            return TypedResults.NotFound();
        }
        var filmUpdate = mapper.Map<Film>(updateFilmsDto);
        filmUpdate.Id = id;
        filmUpdate.Poster = filmDb.Poster;
        
        if (updateFilmsDto.Poster is not null)
        {
            var url = await fileStorage.Storage(_container, updateFilmsDto.Poster);
            filmUpdate.Poster = url;
        }
        await repositoryFilms.UpdateFilm(filmUpdate);
        await outputCacheStore.EvictByTagAsync("films-get", default);
        return TypedResults.NoContent();
    }
    
    static async Task<Results<NoContent,NotFound>> DeleteFilm(int id, IRepositoryFilms repositoryFilms, IOutputCacheStore outputCacheStore, IFileStorage fileStorage)
    {
        var film = await repositoryFilms.GetFilmById(id);
        if (film is null)
        {
            return TypedResults.NotFound();
        }
        await repositoryFilms.DeleteFilm(id);
        await fileStorage.Delete(film.Poster, _container);
        await outputCacheStore.EvictByTagAsync("films-get", default);
        return TypedResults.NoContent();
    }
    
}