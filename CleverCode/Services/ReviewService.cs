using AutoMapper;
using CleverCode.Data;
using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CleverCode.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ReviewService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return new ServiceResult()
            {
                Data = _mapper.Map<List<ReviewDto>>(reviews),
                Message = "Reviews retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> GetReviewByIdAsync(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Review_ID == id);
            return new ServiceResult()
            {
                Data = _mapper.Map<ReviewDto>(review),
                Message = review != null ? "Review retrieved successfully" : "Review not found",
                StatusCode = review != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                Success = review != null
            };
        }
        public async Task<ServiceResult> CreateReviewAsync(ReviewDto reviewDto)
        {
            var reviewEntity = _mapper.Map<Review>(reviewDto);
            var entity = await _context.Reviews.AddAsync(reviewEntity);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ServiceResult()
                {
                    Message = "Couldn't Create Review",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            return new ServiceResult()
            {
                Data = _mapper.Map<ReviewDto>(entity.Entity),
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
                return new ServiceResult()
                {
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            var reviewEntity = _mapper.Map<Review>(reviewDto);
            var updatedEntity = _context.Reviews.Update(reviewEntity);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
                return new ServiceResult()
                {
                    Message = "Couldn't Update Review",
                    StatusCode = HttpStatusCode.BadRequest,
                };
            return new ServiceResult()
            {
                Data = _mapper.Map<ReviewDto>(updatedEntity.Entity),
                Message = "Review Updated successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Review deleted successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }
            return new ServiceResult()
            {
                Message = "Review not found",
                StatusCode = HttpStatusCode.NotFound
            };
        }
        public async Task<ServiceResult> GetReviewsByProjectIdAsync(int serviceId)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.Service_ID == serviceId);
            if (service == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Service not found",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            var reviews = await _context.Reviews
                .Where(r => r.Service_ID == serviceId)
                .ToListAsync();
            return new ServiceResult()
            {
                Data = _mapper.Map<List<ReviewDto>>(reviews),
                Message = "Reviews retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
        public async Task<ServiceResult> ApproveReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            review.IsApproved = true;
            _context.Reviews.Update(review);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Failed to approve review",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResult()
            {
                Data = _mapper.Map<ReviewDto>(review),
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
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Review not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            _context.Reviews.Remove(review);
            var result = await _context.SaveChangesAsync();
            if (result < 0)
            {
                return new ServiceResult()
                {
                    Data = null,
                    Message = "Failed to reject review",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResult()
            {
                Data = null,
                Message = "Review rejected successfully",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };
        }
    }
}
