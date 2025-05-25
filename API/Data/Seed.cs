using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = File.ReadAllText("Data/UserSeedData.json");

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var appUsers = JsonSerializer.Deserialize<IEnumerable<AppUser>>(userData, jsonOptions);

            if (appUsers != null)
            {
                foreach (var user in appUsers)
                {
                    using HMACSHA512 hmac = new HMACSHA512();

                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    user.PasswordSalt = hmac.Key;

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
