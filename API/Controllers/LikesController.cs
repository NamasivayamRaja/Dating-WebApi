using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Repository.Interface;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(ILikeRepository likeRepository) : BaseController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId,
            CancellationToken cancellationToken = default)
        {
            var sourceUserId = User.GetUserId();

            if (sourceUserId == targetUserId)
                return BadRequest("You cannot like yourself");

            var userLike = await likeRepository.GetUserLike(sourceUserId, targetUserId);

            if (userLike is not null)
            {
                likeRepository.DeleteLike(userLike);
            }
            else
            {
                userLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };
                likeRepository.AddLike(userLike);
            }

            if (await likeRepository.SaveChangesAsync(cancellationToken))
                return Ok();

            return BadRequest("Failed to toggle like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            var currentUserId = User.GetUserId();
            var likeIds = await likeRepository.GetUserLikeIdsAsync(currentUserId);
            return Ok(likeIds);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUserLikes([FromQuery]
            LikesParam likesParam,
            CancellationToken cancellationToken = default)
        {
            likesParam.UserId = User.GetUserId();
            var likes = await likeRepository.GetUserLikesAsync(likesParam, cancellationToken);

            Response.AddPaginationHeader(likes);

            return Ok(likes);
        }

    }
}
