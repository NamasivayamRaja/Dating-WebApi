using API.DTOs;
using API.Entities;
using API.Helper;
using API.Interfaces;

namespace API.Repository.Interface
{
    public interface IUserRepository<T> : IRepository<T>
    {
        Task<AppUser?> GetByUserNameAsync(string userName);
        Task<PagedList<MemberDTO>> GetMemberAll(UserParam userParam);
        Task<MemberDTO?> GetMemberByUserNameAsync(string userName);
    }
}
