using CleverCode.DTO;
using CleverCode.Helpers;

namespace CleverCode.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResult> GetAllReviewsAsync();
        //Task<ServiceResult> GetReviewByIdAsync(int id);
        Task<ServiceResult> GetApprovedReviewsAsync();

        Task<ServiceResult> CreateReviewAsync(ReviewDto reviewDto);
        Task<ServiceResult> UpdateReviewAsync(int id, ReviewDto reviewDto);
        //Task<ServiceResult> DeleteReviewAsync(int id);
        Task<ServiceResult> GetReviewsByServiceIdAsync(int serviceId);
        Task<ServiceResult> ApproveReview(int reviewId);
        Task<ServiceResult> RejectReview(int reviewId);
    }
}