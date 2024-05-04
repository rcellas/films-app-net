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
        CreateMap<CreateGenderDTO, GenderFilms>();
        CreateMap<GenderFilms, GenderDTO>();
        
        // Mapeamos Actor con ActorDTO
        // lo que hacemos con el ForMember es decirle que ignore la propiedad Photo
        CreateMap<CreateActorsDTO, Actor>().ForMember(x=>x.Photo, options=>options.Ignore());
        CreateMap<Actor, ActorsDto>();
        
        CreateMap<CreateFilmsDto, Film>().ForMember(x=>x.Poster, options=>options.Ignore());
        CreateMap<Film, FilmsDto>();

        CreateMap<CreateCommentsDto, Comment>();
        CreateMap<Comment, CommentsDto>();
    }
}