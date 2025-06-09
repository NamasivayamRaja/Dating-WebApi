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
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseController
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

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.UserName.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                UserName = user.UserName,
                Token = await tokenService.CreateToken(user),
                Gender = user.Gender.ToString(),
                KnownAs = user.KnownAs,
            };
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await userManager.Users.Include(u=>u.Photos)
                .FirstOrDefaultAsync(u => u.NormalizedUserName == loginDTO.UserName.ToUpper());

            if (user == null || user.UserName == null) { return Unauthorized("Invalid User"); }

            if (!await userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return Unauthorized();
            }

            return new UserDTO
            {
                UserName = user.UserName,
                Token = await tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
                Gender = user.Gender.ToString(),
                KnownAs = user.KnownAs,
            };
        }

        private async Task<bool> CheckUserNameExist(string userName)
        {
            return await userManager.Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper());
        }
    }
}
