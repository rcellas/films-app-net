using AutoMapper;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsUdemy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GendersController : ControllerBase
    {
        private readonly IRespostoryGenderFilm _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public GendersController(IRespostoryGenderFilm repository, IMapper mapper, IMemoryCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> GetAllGenders()
        {
            var cacheKey = "genders";
            List<GenderDTO> gendersDtos;

            // Intenta obtener los datos de la caché
            if (!_cache.TryGetValue(cacheKey, out gendersDtos))
            {
                // Si no están en la caché, obtén los datos de la base de datos
                var genders = await _repository.GetAll();
                gendersDtos = _mapper.Map<List<GenderDTO>>(genders);

                // Guarda los datos en la caché por 60 segundos
                _cache.Set(cacheKey, gendersDtos, TimeSpan.FromSeconds(60));
            }

            return Ok(gendersDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenderDTO>> GetGenderById(int id)
        {
            var gender = await _repository.GetById(id);
            if (gender == null)
            {
                return NotFound();
            }
            var genderDto = _mapper.Map<GenderDTO>(gender);
            return Ok(genderDto);
        }

        [HttpPost]
        public async Task<ActionResult<GenderDTO>> CreateGender(CreateGenderDTO createGenderDto)
        {
            var gender = _mapper.Map<GenderFilms>(createGenderDto);
            var id = await _repository.Create(gender);
            var genderDto = _mapper.Map<GenderDTO>(gender);
            return Created($"/api/genders/{id}", genderDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateGender(int id, CreateGenderDTO updateDto)
        {
            var exist = await _repository.Exist(id);
            if (!exist)
            {
                return NotFound();
            }
            var gender = _mapper.Map<GenderFilms>(updateDto);
            gender.Id = id;
            await _repository.Update(gender);
            _cache.Remove("genders"); // Remover la cache para refrescar los datos
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGender(int id)
        {
            var exist = await _repository.Exist(id);
            if (!exist)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            _cache.Remove("genders"); // Remover la cache para refrescar los datos
            return NoContent();
        }
    }
}
