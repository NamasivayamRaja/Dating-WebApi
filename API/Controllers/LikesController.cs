using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helper;
using API.Interfaces;
using API.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController(IUnitOfWork unitOfWork) : BaseController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId,
            CancellationToken cancellationToken = default)
        {
            var sourceUserId = User.GetUserId();

            if (sourceUserId == targetUserId)
                return BadRequest("You cannot like yourself");

            var userLike = await unitOfWork.LikeRepository.GetUserLike(sourceUserId, targetUserId);

            if (userLike is not null)
            {
                unitOfWork.LikeRepository.DeleteLike(userLike);
            }
            else
            {
                userLike = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };
                unitOfWork.LikeRepository.AddLike(userLike);
            }

            if (await unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to toggle like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            var currentUserId = User.GetUserId();
            var likeIds = await unitOfWork.LikeRepository.GetUserLikeIdsAsync(currentUserId);
            return Ok(likeIds);
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUserLikes([FromQuery]
            LikesParam likesParam,
            CancellationToken cancellationToken = default)
        {
            likesParam.UserId = User.GetUserId();
            var likes = await unitOfWork.LikeRepository.GetUserLikesAsync(likesParam, cancellationToken);

            Response.AddPaginationHeader(likes);

            return Ok(likes);
        }

    }
}
