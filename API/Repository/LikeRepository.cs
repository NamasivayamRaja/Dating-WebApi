using API.Data;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Repository.Interface;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class LikeRepository(DataContext context, IMapper mapper) : ILikeRepository
    {
        public void AddLike(UserLike userLike)
        {
            context.Likes.Add(userLike);
        }

        public void DeleteLike(UserLike userLike)
        {
            context.Likes.Remove(userLike);
        }

        public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await context.Likes
                .FindAsync(sourceUserId, targetUserId);
        }

        public async Task<IEnumerable<int>> GetUserLikeIdsAsync(int currentUserId)
        {
            return await context.Likes
                .Where(l => l.SourceUserId == currentUserId)
                .Select(l => l.TargetUserId)
                .ToListAsync();
        }

        public async Task<PagedList<MemberDTO>> GetUserLikesAsync(LikesParam likesParam, CancellationToken cancellationToken = default)
        {
            var likes =  context.Likes.AsQueryable();
            IQueryable<MemberDTO> query;

            switch (likesParam.Predicate) {
                case "liked":
                    query = likes.Where(l => l.SourceUserId == likesParam.UserId)
                        .Select(t => t.TargetUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                    break;
                case "likedBy":
                    query = likes.Where(l => l.TargetUserId == likesParam.UserId)
                        .Select(t => t.SourceUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                        break;
                default:
                    var likeIds = await GetUserLikeIdsAsync(likesParam.UserId);

                    query = likes.Where(x=>x.TargetUserId == likesParam.UserId && likeIds.Contains(x.SourceUserId))
                        .Select(t => t.SourceUser)
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider);
                    break;
            }

            return await PagedList<MemberDTO>.CreateAsync
                (
                    query,
                    likesParam.PageNumber, 
                    likesParam.PageSize
                );
        }
    }
}
