using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService) : BaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) 
        {
            if (await CheckUserNameExist(registerDTO.UserName)) { return BadRequest("User name already registered"); }

            using HMACSHA512 hmac = new HMACSHA512();

            //var user = new AppUser
            //{
            //    UserName = registerDTO.UserName.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            //    PasswordSalt = hmac.Key
            //};

            //context.Users.Add(user);
            //await context.SaveChangesAsync();

            //return new UserDTO
            //{
            //    UserName = user.UserName,
            //    Token = tokenService.CreateToken(user)
            //};
            return Ok();
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
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
            };
        }

        private async Task<bool> CheckUserNameExist(string userName)
        {
            return await context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
    }
}
