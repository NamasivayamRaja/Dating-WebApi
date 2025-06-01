using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await CheckUserNameExist(registerDTO.UserName)) { return BadRequest("User name already registered"); }

            using HMACSHA512 hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return new UserDTO
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender.ToString(),
                KnownAs = user.KnownAs,
            };
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await context.Users.Include(u=>u.Photos).FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName.ToLower());

            if (user == null) { return Unauthorized("Invalid User"); }

            using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            var computePassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computePassword.Length; i++)
            {
                if (computePassword[i] != user.PasswordHash[i]) { return Unauthorized("Invalid Password"); }
            }

            return new UserDTO
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
                Gender = user.Gender.ToString(),
                KnownAs = user.KnownAs,
            };
        }

        private async Task<bool> CheckUserNameExist(string userName)
        {
            return await context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}
