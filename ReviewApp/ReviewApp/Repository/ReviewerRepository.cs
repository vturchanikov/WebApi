using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : Repository, IReviewerRepository
    {
        public ReviewerRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
            
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviwers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviwers.ToList();
        }

        public ICollection<Review> GetReviewsByReviwer(int reviewerId)
        {
            return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviwers.Any(r => r.Id == reviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);

            return Save();
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);

            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);

            return Save();
        }
    }
}
