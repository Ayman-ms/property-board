using Microsoft.AspNetCore.Mvc;
using Backend.Api.Repository;
using Backend.Api.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Backend.Api.Controllers
{
    [Authorize] // حماية المسار: يتطلب Token
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoritesController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        [HttpPost("{propertyId}")]
        public async Task<ActionResult<ApiResponse<bool>>> ToggleFavorite(int propertyId)
        {
            try
            {
                // استخراج معرف المستخدم من الـ Claims في التوكن
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized(ApiResponse<bool>.Failure("Unauthorized"));
                
                int userId = int.Parse(userIdClaim.Value);

                if (await _favoriteRepository.IsFavoriteAsync(userId, propertyId))
                {
                    await _favoriteRepository.RemoveFavoriteAsync(userId, propertyId);
                    return Ok(ApiResponse<bool>.Success(false, "Removed from favorites"));
                }
                else
                {
                    await _favoriteRepository.AddFavoriteAsync(userId, propertyId);
                    return Ok(ApiResponse<bool>.Success(true, "Added to favorites"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Failure(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetMyFavorites()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);
                return Ok(ApiResponse<IEnumerable<object>>.Success(favorites));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<object>>.Failure(ex.Message));
            }
        }
    }
}