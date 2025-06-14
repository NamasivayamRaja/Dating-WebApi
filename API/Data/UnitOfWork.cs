using API.Entities;
using API.Interfaces;
using API.Repository.Interface;

namespace API.Data
{
    public class UnitOfWork(DataContext dataContext
        ,IUserRepository<AppUser> userRepository
        ,IMessageRepository messageRepository,
        ILikeRepository likeRepository) : IUnitOfWork
    {
        public IUserRepository<AppUser> UserRepository => userRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public ILikeRepository LikeRepository => likeRepository;

        public async Task<bool> Complete()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return dataContext.ChangeTracker.HasChanges();
        }
    }
}
