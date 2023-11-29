using ChatWeb.Application.Constants;
using ChatWeb.Domain.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatWeb.Presistence;

public static class OnlineTestingDbSeeder
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
            context.Database.Migrate();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<UserEntity>>();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<RoleEntity>>();

            if (!context.Roles.Any())
            {
                foreach (var role in Roles.All)
                {
                    var result = roleManager.CreateAsync(new RoleEntity
                    {
                        Name = role
                    }).Result;
                }
            }

            if (!context.Users.Any())
            {
                UserEntity user = new()
                {
                    Email = "admin@localhost",
                    UserName = "admin"
                };
                var result = userManager.CreateAsync(user, "123456")
                    .Result;
                if (result.Succeeded)
                {
                    result = userManager
                        .AddToRoleAsync(user, Roles.Admin)
                        .Result;
                }
            }
        }
    }
}
