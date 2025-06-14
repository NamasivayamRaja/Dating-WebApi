using API.Entities;
using API.Repository.Interface;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository<AppUser> UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        ILikeRepository LikeRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
