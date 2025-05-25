using API.DTOs;
using API.Entities;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IUserRepository<AppUser> repository, IMapper mapper) : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await repository.GetMemberAll();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetUserById(int id)
        {
            var user = await repository.GetByIdAsync(id);

            if (user == null) 
            {
                return NotFound();
            }

            var userReturn = mapper.Map<MemberDTO>(user);

            return Ok(userReturn);
        }

        [HttpGet("userName/{userName}")] // api/users/raja
        public async Task<ActionResult<MemberDTO>> GetUserByName(string userName)
        {
            var user = await repository.GetMemberByUserNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}