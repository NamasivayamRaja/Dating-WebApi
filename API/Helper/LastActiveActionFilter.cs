using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helper
{
    public class LastActiveActionFilter : IAsyncActionFilter
    {
        private readonly IUserRepository<AppUser> _repository;

        public LastActiveActionFilter(IUserRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

            var userId = context.HttpContext.User.GetUserId();
            var user = await _repository.GetByIdAsync(userId);

            if (user != null)
            {
                user.LastActive = DateTime.UtcNow;
                await _repository.UpdateAsync(user);
            }
        }
    }
}
