using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Repository
{
    public interface IUserRepository<T> : IRepository<T>
    {
        Task<AppUser?> GetByUserNameAsync(string userName);
        Task<IEnumerable<MemberDTO>> GetMemberAll();
        Task<MemberDTO?> GetMemberByUserNameAsync(string userName);
    }
}
