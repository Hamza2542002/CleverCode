using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CleverCode.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ReviewService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string GetLanguage()
        {
            var lang = _httpContextAccessor.HttpContext?.Request?.Headers["Accept-Language"].ToString().ToLower();
            return lang == "ar" ? "ar" : "en";
        }

        private LocalizedReviewDto LocalizeReview(Review review)
        {
            var lang = GetLanguage();

            return new LocalizedReviewDto
            {
                Review_ID = review.Review_ID,
                Comment = lang == "ar" ? review.CommentAr : review.CommentEn,
                Rate = review.Rate,
                Date = review.Date,
                Name = review.Name,
                Company = review.Company,
                IsApproved = review.IsApproved,
                Service_ID = review.Service_ID
            };
        }

        public async Task<ServiceResult> GetAllReviewsAsync()
        {
            var lang = GetLanguage();
            var reviews = await _context.Reviews.ToListAsync();

            var localized = reviews.Select(r => LocalizeReview(r)).ToList();

            return new ServiceResult
            {
                Data = localized,
                Message = "Reviews retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        //public async Task<ServiceResult> GetReviewByIdAsync(int id)
        //{
        //    var r = await _context.Reviews.FindAsync(id);

        //    if (r == null)
        //    {
        //        return new ServiceResult
        //        {
        //            Message = "Review not found",
        //            StatusCode = HttpStatusCode.NotFound
        //        };
        //    }

        //    return new ServiceResult
        //    {
        //        Data = LocalizeReview(r),
        //        Message = "Review retrieved successfully",
        //        StatusCode = HttpStatusCode.OK,
        //        Success = true
        //    };
        //}
        public async Task<ServiceResult> GetApprovedReviewsAsync()
        {
            var approvedReviews = await _context.Reviews
                .Where(r => r.IsApproved)
                .ToListAsync();

            var localized = approvedReviews.Select(r => LocalizeReview(r)).ToList();

            return new ServiceResult
            {
                Data = localized,
                Message = "Approved reviews retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> GetReviewsByServiceIdAsync(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);

            if (service == null)
            {
                return new ServiceResult
                {
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var reviews = await _context.Reviews.Where(r => r.Service_ID == serviceId).ToListAsync();

            var localized = reviews.Select(r => LocalizeReview(r)).ToList();

            return new ServiceResult
            {
                Data = localized,
                Message = "Reviews retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> CreateReviewAsync(ReviewDto reviewDto)
        {
            var reviewEntity = _mapper.Map<Review>(reviewDto);
            await _context.Reviews.AddAsync(reviewEntity);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return new ServiceResult
                {
                    Message = "Couldn't create review",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResult
            {
                Data = LocalizeReview(reviewEntity),
                Message = "Review created successfully",
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }

        public async Task<ServiceResult> UpdateReviewAsync(int id, ReviewDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return new ServiceResult
                {
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            review.CommentAr = reviewDto.CommentAr;
            review.CommentEn = reviewDto.CommentEn;
            review.Rate = reviewDto.Rate;
            review.Date = reviewDto.Date;
            review.Name = reviewDto.Name;
            review.Company = reviewDto.Company;
            review.Service_ID = reviewDto.Service_ID;

            _context.Reviews.Update(review);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return new ServiceResult
                {
                    Message = "Couldn't update review",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return new ServiceResult
            {
                Data = LocalizeReview(review),
                Message = "Review updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        //public async Task<ServiceResult> DeleteReviewAsync(int id)
        //{
        //    var review = await _context.Reviews.FindAsync(id);
        //    if (review == null)
        //    {
        //        return new ServiceResult
        //        {
        //            Message = "Review not found",
        //            StatusCode = HttpStatusCode.NotFound
        //        };
        //    }

        //    _context.Reviews.Remove(review);
        //    await _context.SaveChangesAsync();

        //    return new ServiceResult
        //    {
        //        Message = "Review deleted successfully",
        //        StatusCode = HttpStatusCode.OK,
        //        Success = true
        //    };
        //}

        public async Task<ServiceResult> ApproveReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ServiceResult
                {
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            review.IsApproved = true;
            _context.Reviews.Update(review);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return new ServiceResult
                {
                    Message = "Failed to approve review",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return new ServiceResult
            {
                Data = LocalizeReview(review),
                Message = "Review approved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }

        public async Task<ServiceResult> RejectReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ServiceResult
                {
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _context.Reviews.Remove(review);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return new ServiceResult
                {
                    Message = "Failed to reject review",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return new ServiceResult
            {
                Message = "Review rejected successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
