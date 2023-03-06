using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IReviewerRepository
{
    ICollection<Reviewer> GetReviewers();

    Reviewer GetReviewer(int reviwerId);
    ICollection<Review> GetReviewsByReviwer(int reviewerId);
    bool ReviewerExists(int reviewerId);

    bool CreateReviewer(Reviewer reviewer);
    bool UpdateReviewer(Reviewer reviewer);
    bool DeleteReviewer(Reviewer reviewer);

    bool Save();
}
