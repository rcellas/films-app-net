using AutoMapper;
using FilmsUdemy.DTOs;
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
    }
}