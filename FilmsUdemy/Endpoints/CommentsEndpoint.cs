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
        group.MapPost("/", CreateComment);
        return group;
    }

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
}