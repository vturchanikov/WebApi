using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _reviewerRepository = reviewerRepository;
        _pokemonRepository = pokemonRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType((400))]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(reviews);
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType((400))]
    public IActionResult GetReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

        var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(review);
    }

    [HttpGet("pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType((400))]
    public IActionResult GetReviewsForAPokemon(int pokeId)
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(reviews); 
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
    {
        if (reviewCreate == null)
        {
            return BadRequest(ModelState);
        }

        var reviews = _reviewRepository.GetReviews()
            .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.Trim().ToUpper())
            .FirstOrDefault();

        if (reviews != null)
        {
            ModelState.AddModelError("", "Review already exist");

            return StatusCode(422, ModelState);
        }

        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var reviewMap = _mapper.Map<Review>(reviewCreate);

        reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong saving");

            return StatusCode(500, ModelState);
        }

        return Ok("Succesfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
    {
        if (updatedReview == null)
            return BadRequest();

        if (reviewId != updatedReview.Id)
            return BadRequest(ModelState);

        if (!_reviewerRepository.ReviewerExists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewMap = _mapper.Map<Review>(updatedReview);

        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating reviewer");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
        {
            return NotFound();
        }

        var reviewToDelete = _reviewRepository.GetReview(reviewId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_reviewRepository.DeleteReview(reviewToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting owner");
        }

        return NoContent();
    }

}
