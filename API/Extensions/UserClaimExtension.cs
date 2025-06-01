using System.Security.Claims;

namespace API.Extensions
{
    public static class UserClaimExtension
    {
        public static string GetUserName(this ClaimsPrincipal principal) 
        {
            return principal.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("User not available in the token.");
        }

        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User not available in the token."));
        }
    }
}
