using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repository;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IMapper _mapper;

    public OwnerController(IOwnerRepository ownerRepository,ICountryRepository countryRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository;
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public IActionResult GetOwners()
    {
        var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(owners);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType((400))]
    public IActionResult GetPokemon(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
            return NotFound();

        var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(owner);
    }

    [HttpGet("{ownerId}/pokemon")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType((400))]
    public IActionResult GetPokemonByOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
        {
            return NotFound();
        }

        var pokemon = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

        if (ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pokemon);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromQuery] int countryId, [FromBody] OwnerDto ownerCreate)
    {
        if (ownerCreate == null)
        {
            return BadRequest(ModelState);
        }

        var owners = _ownerRepository.GetOwners()
            .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper())
            .FirstOrDefault();

        if (owners != null)
        {
            ModelState.AddModelError("", "Owner already exist");

            return StatusCode(422, ModelState);
        }

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var ownerMap = _mapper.Map<Owner>(ownerCreate);

        ownerMap.Country = _countryRepository.GetCountry(countryId);

        if (!_ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong saving");

            return StatusCode(500, ModelState);
        }

        return Ok("Succesfully created");
    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwner)
    {
        if (updatedOwner == null)
            return BadRequest();

        if (ownerId != updatedOwner.Id)
            return BadRequest(ModelState);

        if (!_countryRepository.CountryExists(ownerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var ownerMap = _mapper.Map<Owner>(updatedOwner);

        if (!_ownerRepository.UpdateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating owner");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOwner(int ownerId)
    {
        if (!_ownerRepository.OwnerExists(ownerId))
        {
            return NotFound();
        }

        var ownerToDelete = _ownerRepository.GetOwner(ownerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_ownerRepository.DeleteOwner(ownerToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting owner");
        }

        return NoContent();
    }
}
