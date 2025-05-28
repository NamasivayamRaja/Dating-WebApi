using System.Security.Claims;

namespace API.Extensions
{
    public static class UserClaimExtension
    {
        public static string GetUserName(this ClaimsPrincipal principal) 
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User not available in the token.");
        }
    }
}
