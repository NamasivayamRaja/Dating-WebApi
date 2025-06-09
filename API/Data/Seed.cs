using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = File.ReadAllText("Data/UserSeedData.json");

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var appUsers = JsonSerializer.Deserialize<IEnumerable<AppUser>>(userData, jsonOptions);

            var roles = new List<AppRole>
            {
                new() { Name = "Member" },
                new() { Name = "Admin" },
                new() { Name = "Moderator" }
            };

            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            if (appUsers != null)
            {
                foreach (var user in appUsers)
                {
                    user.UserName = user.UserName!.ToLower();
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "Member");
                }
            }

            var admin = new AppUser
            {
                KnownAs = "Admin",
                UserName = "admin",
                City = "",
                Gender = (int)Gender.SystemUser,
                Country = "",
                DateOfBirth = new DateOnly(1990, 1, 1)
            };

            if (await userManager.FindByNameAsync(admin.UserName) == null)
            {
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
            }
        }
    }
}
