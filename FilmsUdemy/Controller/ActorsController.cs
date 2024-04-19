using AutoMapper;
using FilmsUdemy.DTOs.Actors;
using FilmsUdemy.Entity;
using FilmsUdemy.Repositories.Actors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FilmsUdemy.Service;

namespace FilmsUdemy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly IRepositoryActors _repositoryActors;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private static readonly string container = "actors";

        public ActorsController(IRepositoryActors repositoryActors, IMapper mapper, IFileStorage fileStorage)
        {
            _repositoryActors = repositoryActors;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorsDTO>>> GetAllActors()
        {
            var actors = await _repositoryActors.GetAllActors();
            var actorsDtos = _mapper.Map<List<ActorsDTO>>(actors);
            return Ok(actorsDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorsDTO>> GetActorById(int id)
        {
            var actor = await _repositoryActors.GetActorById(id);
            if (actor == null)
            {
                return NotFound();
            }
            var actorDto = _mapper.Map<ActorsDTO>(actor);
            return Ok(actorDto);
        }

        [HttpPost]
        public async Task<ActionResult<ActorsDTO>> CreateActor([FromForm] CreateActorsDTO createActorsDto)
        {
            var actor = _mapper.Map<Actor>(createActorsDto);
            if (createActorsDto.Photo != null)
            {
                string url = await _fileStorage.Storage(container, createActorsDto.Photo);
                actor.Photo = url;
            }
            int id = await _repositoryActors.CreateActor(actor);
            var actorDTO = _mapper.Map<ActorsDTO>(actor);
            return Created($"/api/actors/{id}", actorDTO);
        }
    }
}
