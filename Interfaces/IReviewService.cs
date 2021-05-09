using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IReviewService
    {
        bool DeleteReview(int id);
        ICollection<Review> GetAll();
        Review GetReview(int id);
        Review PostReview(Review review);
        bool PutReview(Review review);
    }
}