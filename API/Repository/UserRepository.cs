using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entities;
using AutoMapper;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using API.Helper;
using API.Extensions;
using API.Enums;
using API.Repository.Interface;

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
                .FindAsync(id);
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

        public async Task<PagedList<MemberDTO>> GetMemberAll(UserParam userParam)
        {
            var query = dataContext.Users.AsQueryable();

            query = query.Where(x => x.UserName != userParam.UserName);

            if (userParam.Gender.HasValue && userParam.Gender.Value != Gender.All)
            {
                query = query.Where(x => x.Gender == (int)userParam.Gender.Value);
            }
            else
            {
                query = query.Where(x => x.Gender != (int)Gender.SystemUser);
            }

            var minDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParam.MaxAge - 1));
            var maxDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParam.MinAge));

            query = query.Where(x => x.DateOfBirth >= minDate && x.DateOfBirth <= maxDate);

            query = userParam.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.CreatedDateTime),
                _ => query.OrderByDescending(x => x.LastActive)
            };

            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(mapper.ConfigurationProvider), userParam.PageNumber, userParam.PageSize);
        }

        public async Task<MemberDTO?> GetMemberByUserNameAsync(string userName)
        {
            return await dataContext.Users
                .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }
    }
}
