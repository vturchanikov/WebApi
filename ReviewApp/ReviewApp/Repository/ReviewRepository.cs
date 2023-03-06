using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

public class ReviewRepository : Repository, IReviewRepository
{
    public ReviewRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {

    }

    public Review GetReview(int reviewId)
    {
        return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
    }

    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.ToList();
    }

    public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
    {
        return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
    }

    public bool ReviewExists(int reviewId)
    {
        return _context.Reviews.Any(r => r.Id == reviewId);
    }

    public bool CreateReview(Review review)
    {
        _context.Add(review);

        return Save();
    }

    public bool UpdateReview(Review review)
    {
        _context.Update(review);

        return Save();
    }

    public bool DeleteReview(Review review)
    {
        _context.Remove(review);

        return Save();
    }

    public bool DeleteReviews(List<Review> reviews)
    {
        _context.RemoveRange(reviews);

        return Save();
    }
}
