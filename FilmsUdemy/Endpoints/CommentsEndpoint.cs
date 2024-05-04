using AutoMapper;
using FilmsUdemy.DTOs.Comment;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories.CommentsFilms;
using FilmsUdemy.Repositories.Films;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace FilmsUdemy.Endpoints;

public static class CommentsEndpoint
{
    public static RouteGroupBuilder MapComments(this RouteGroupBuilder group)
    {
        // cuando usamos el SetVaryByRouteValue, le estamos diciendo que si el valor de la ruta cambia, entonces la cache se invalida
        group.MapGet("/", GetAllComments).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(60)).Tag("comments-get").SetVaryByRouteValue(new string[] {"filmId"}));
        group.MapGet("/{id:int}", GetCommentById);
        group.MapPost("/", CreateComment);
        group.MapPut("/{id:int}", UpdateComment);
        group.MapDelete("/{id:int}", DeleteComment);
        return group;
    }
    
    // get
    static async Task<Results<Ok<List<CommentsDto>>,NotFound>> GetAllComments(int filmId,
        IRepositoriesCommentsFilms repositoriesCommentsFilms,IRepositoryFilms repositoryFilms, IMapper mapper)
    {
        if(!await repositoryFilms.ExistFilm(filmId))
        {
            return TypedResults.NotFound();
        }
        var comments = await repositoriesCommentsFilms.GetAllComments(filmId);
        var commentsDto = mapper.Map<List<CommentsDto>>(comments);
        return TypedResults.Ok(commentsDto);
    }
    
    static async Task<Results<Ok<CommentsDto>,NotFound>> GetCommentById(int id, int filmId,
        IRepositoriesCommentsFilms repositoriesCommentsFilms, IMapper mapper)
    {
        var comment = await repositoriesCommentsFilms.GetCommentById(id);
        if (comment is null)
        {
            return TypedResults.NotFound();
        }
        var commentDto = mapper.Map<CommentsDto>(comment);
        return TypedResults.Ok(commentDto);
    }
    
    // post
    static async Task<Results<Created<CommentsDto>, NotFound>> CreateComment(int filmId,
        CreateCommentsDto createCommentsDto, IRepositoriesCommentsFilms repositoriesCommentsFilms,
        IRepositoryFilms repositoryFilms, IMapper mapper, IOutputCacheStore outputCacheStore)
    {
        if (!await repositoryFilms.ExistFilm(filmId))
        {
            return TypedResults.NotFound();
        }
        var comment = mapper.Map<Comment>(createCommentsDto);
        comment.FilmId = filmId;
        var id = await repositoriesCommentsFilms.CreateComment(comment);
        await outputCacheStore.EvictByTagAsync("comments-get",default);
        var commentDto = mapper.Map<CommentsDto>(comment);
        return TypedResults.Created($"/comments/{id}", commentDto);
    }
    
    // put
    static async Task<Results<NoContent, NotFound>> UpdateComment(int id, int filmId,
        CreateCommentsDto updateCommentsDto, IRepositoriesCommentsFilms repositoriesCommentsFilms,
        IRepositoryFilms repositoryFilms, IMapper mapper, IOutputCacheStore outputCacheStore)
    {
        if (!await repositoryFilms.ExistFilm(filmId))
        {
            return TypedResults.NotFound();
        }
        if(!await repositoriesCommentsFilms.ExistComment(id))
        {
            return TypedResults.NotFound();
        }

        var comment = mapper.Map<Comment>(updateCommentsDto);
        comment.Id = id;
        comment.FilmId = filmId;
        
        await repositoriesCommentsFilms.UpdateComment(comment);
        await outputCacheStore.EvictByTagAsync("comments-get",default);
        return TypedResults.NoContent();
    }
    
    // delete
    static async Task<Results<NoContent, NotFound>> DeleteComment(int id, int filmId,
        IRepositoriesCommentsFilms repositoriesCommentsFilms, IRepositoryFilms repositoryFilms, IOutputCacheStore outputCacheStore)
    {
        if(!await repositoriesCommentsFilms.ExistComment(id))
        {
            return TypedResults.NotFound();
        }
        await repositoriesCommentsFilms.DeleteComment(id);
        await outputCacheStore.EvictByTagAsync("comments-get",default);
        return TypedResults.NoContent();
    }
}