using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private ICountryRepository _countryRepository { get; set; }
    private IMapper _mapper { get; set; }

    public CountryController(ICountryRepository countryRepository, IMapper mapper)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    public IActionResult GetCountries()
    {
        var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(countries);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType((400))]
    public IActionResult GetCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(country);
    }

    [HttpGet("/owners/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public IActionResult GetCountyOfAnOwner(int ownerId)
    {
        var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(country);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
    {
        if (countryCreate == null)
        {
            return BadRequest(ModelState);
        }

        var country = _countryRepository.GetCountries()
            .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper())
            .FirstOrDefault();

        if (country != null)
        {
            ModelState.AddModelError("", "Country already exist");

            return StatusCode(422, ModelState);
        }

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var countryMap = _mapper.Map<Country>(countryCreate);

        if (!_countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong saving");

            return StatusCode(500, ModelState);
        }

        return Ok("Succesfully created");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateCategory(int countryId, [FromBody] CountryDto updatedCountry)
    {
        if (updatedCountry == null)
            return BadRequest();

        if (countryId != updatedCountry.Id)
            return BadRequest(ModelState);

        if (!_countryRepository.CountryExists(countryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var countryMap = _mapper.Map<Country>(updatedCountry);

        if (!_countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong updating country");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteCountry(int countryId)
    {
        if (!_countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        var countryToDelete = _countryRepository.GetCountry(countryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_countryRepository.DeleteCountry(countryToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }

        return NoContent();
    }

}
