using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using AutoMapper;
using API.DTOs;
using AutoMapper.QueryableExtensions;

namespace API.Repository
{
    public class UserRepository(DataContext dataContext, IMapper mapper) : IUserRepository<AppUser>
    {
        public async Task<AppUser> CreateAsync(AppUser entity)
        {
            dataContext.Users.Add(entity);
            await dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(AppUser entity)
        {
            dataContext.Users.Remove(entity);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await dataContext.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await dataContext.Users
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=> x.Id == id);
        }

        public async Task<AppUser?> GetByUserNameAsync(string userName)
        {
            return await dataContext.Users
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=>x.UserName == userName);
        }

        public async Task UpdateAsync(AppUser entity)
        {
            dataContext.Entry(entity).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMemberAll()
        {
            return await dataContext.Users
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDTO?> GetMemberByUserNameAsync(string userName)
        {
            return await dataContext.Users
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

    }
}
