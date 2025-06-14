using API.DTOs;
using API.Entities;
using API.Helper;

namespace API.Repository.Interface
{
    public interface ILikeRepository
    {
        Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);
        Task<PagedList<MemberDTO>> GetUserLikesAsync(
            LikesParam likesParam, 
            CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId);
        void AddLike(UserLike userLike);
        void DeleteLike(UserLike userLike);
    }
}
