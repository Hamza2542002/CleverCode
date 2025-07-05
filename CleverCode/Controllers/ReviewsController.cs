using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using CleverCode.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _reviewService.GetAllReviewsAsync();
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpGet("{id}")]
        //public async Task<IActionResult> GetReviewById(int id)
        //{
        //    var result = await _reviewService.GetReviewByIdAsync(id);
        //    if (!result.Success)
        //        return NotFound(new ErrorResponse(result.StatusCode, result.Message));
        //    return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        //}
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedReviews()
        {
            var result = await _reviewService.GetApprovedReviewsAsync();
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));

            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
        {
            var result = await _reviewService.CreateReviewAsync(reviewDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto reviewDto)
        {
            if (reviewDto.Review_ID != id)
                return BadRequest(new ErrorResponse(System.Net.HttpStatusCode.BadRequest, "Review ID mismatch."));
            var result = await _reviewService.UpdateReviewAsync(id, reviewDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReview(int id)
        //{
        //    var result = await _reviewService.DeleteReviewAsync(id);
        //    if (!result.Success)
        //        return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
        //    return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        //}

        [HttpGet("ByService/{serviceId}")]
        public async Task<IActionResult> GetReviewsByServiceId(int serviceId)
        {
            var result = await _reviewService.GetReviewsByServiceIdAsync(serviceId);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost("{reviewId}/approve")]
        public async Task<IActionResult> ApproveReview(int reviewId)
        {
            var result = await _reviewService.ApproveReview(reviewId);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost("{reviewId}/reject")]
        public async Task<IActionResult> RejectReview(int reviewId)
        {
            var result = await _reviewService.RejectReview(reviewId);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }
    }
}