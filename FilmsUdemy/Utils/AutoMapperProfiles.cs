using AutoMapper;
using FilmsUdemy.DTOs;
using FilmsUdemy.DTOs.Actors;
using FilmsUdemy.DTOs.Comment;
using FilmsUdemy.DTOs.Films;
using FilmsUdemy.Entity;

namespace FilmsUdemy.Utils;

// Profile es una clase de AutoMapper que nos permite mapear las entidades
public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        // CreateMap nos permite mapear una entidad con un DTO, en nuestro caso CreateGenderDTO con GenderFilms
        CreateMap<CreateGenderDTO, Gender>();
        CreateMap<Gender, GenderDto>();
        
        // Mapeamos Actor con ActorDTO
        // lo que hacemos con el ForMember es decirle que ignore la propiedad Photo
        CreateMap<CreateActorsDTO, Actor>().ForMember(x=>x.Photo, options=>options.Ignore());
        CreateMap<Actor, ActorsDto>();
        
        CreateMap<CreateFilmsDto, Film>().ForMember(x=>x.Poster, options=>options.Ignore());
        // Lo que estamos haciendo aquí es mapear la entidad Film con el DTO FilmsDto
        // en el caso de gender es one many, por lo que debemos mapear la entidad y el DTO de forma diferente a los demás, como actor
        CreateMap<Film, FilmsDto>().ForMember(p => p.Gender, entity => entity.MapFrom(p =>
                p.GendersFilms.Select(gf => new GenderDto { Id = gf.GenderId, Name = gf.Gender.Name }))).ForMember(p => p.Actor, entity => entity.MapFrom(p => p.ActorFilms.Select(af => new ActorFilmDto { Id = af.ActorId, Name = af.Actor.Name, Character=af.Character })));

        CreateMap<CreateCommentsDto, Comment>();
        CreateMap<Comment, CommentsDto>();

        CreateMap<AsingActorFilmDto, ActorFilm>();
    }
}