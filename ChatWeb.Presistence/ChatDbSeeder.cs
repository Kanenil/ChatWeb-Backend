using ChatWeb.Application.Constants;
using ChatWeb.Application.Contracts.Infrastructure;
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

            var imageService = scope.ServiceProvider
                .GetRequiredService<IImageService>();

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
                var image = imageService.SaveImageFromBase64("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFoAAABaCAYAAAA4qEECAAAAAXNSR0IArs4c6QAABIZJREFUeF7tnF1y2jAQx7WEPJAcouQkhXOETuEOoTN9Cn3qTMkdoFNyjpKThB4iyUMI6siqizEGraSVbJHlFX3+tP5L3tUaBP+iEIAovXAngkFHMgIG3QTQ08FLT4Logtx8iDQe4m5aD+u2WH2dd1bEDVs3t2fR34cv3fZazoQQPevWmlthuW7DqE7gO6Cng6dbIWDSXF5+I5MAo7czsawD+H/Qpw55u0RyMl5cfvNbMvvaGWilxULI3/bVU60B/fGis4w5evinyY8xO627L5Bi9XoO/ZgSAu9HMnaXV+n1l1+deaxFf7egQcr5zf3lKCLoZ6XNp3SUw7JbjhcXfWxh33IwHTxLcyMywSOf+Zg6XlxEezNGgY45IPOi40pgDCjmvBg0bt28SzFob4S4Bhg0jpN3KQbtjRDXAIPGcfIuxaC9EeIaYNA4Tt6lGPQBhMrZdvYmevjo0vFoDoOuAD0deLklKqM5DLoEmsqbWY7mMOgCaHrf/Daaw6ALoKmsefch0dEcBl2g8uPTyxBkdgOA7JdHcxj0jkWHiZ0qvWbQgTbDYrMqmsOgK0Ti7vppJgGGZPohBGv0IZj6CsbmIw62OZrDFo0jebQUJprDoBm0HwGMlVHFDDF9sUX7rWdWm0EbILJFR7IyBs2gCQhEagKjm2zRBIvBoAkgYppg0BhKBGUYNAFETBMMGkOJoEzjQN9dPz+qpE3fualIggSxcsnnU54ykJvPQkCWQOo7Fkx9Nd6b+4srTFlTGcyiqlfwADf+8VlPYeJ0JjTZ/2Q3/lGgAzi5BSbrqf4MXbp8QxToEAFJZS7Hsp6akNe4bsMVVfobCrT2PtGnJh8CHaIvlFAUClFnZKFBqzFQbYrb+ew+mvVLhR4Z5SaYz9UKtN755Yxu19+CboJU5JDVk0adnmwFOl8dus1Rg26CVGjI4RI4nUBrzc4jwNADKbouVq4mJgHUmbi2ZFE9BrESovVAbcXFfcEZtGlzCWylZOdb0zyo/k8QNN3Zlgoipp1kQOvXd/pNCgOJokwqoJOTivLiJAA6TalIBnTqUpEK6OSlIgHQpyEVjQV9alIRDbSla9UoFcrhdP66UR83JIuw+ER8qMejxuL0qRu8k8gsFfi2fE68crJut35i/M+BxrN0Am3yYWOlgj6v7+hiGL9PGm48cuIMWvuw93M9bLxkgaznIG2TAYQaj4rmeIEuevoktP7YfmA1sHPqiHlXS0mI8eSG5w3aRzktN1Wfrqrq7kkJNehiNKdW0Fp+aO6VuKxCWUooQZfbrh20mhxIGLoEF1zgVtfRUtJeZ5d4vD+mWLVP1Q56q/Py1jWaQwR8CVIqd61TIqcpmtMI0ESgsmbCR9vN7wZV8zk50Pkkw0kSg94zJPorFKoLBl2pQPRSwqCPSj2dlDBo455KIyUM2gia5lTCoFGg/U8lDNoKdP6iZH+xk0Fbg3aTEgbtBNpWSlwzBU72zdCFuulUYhPUKPfPoEtEDr3g+EBWXTDoA6af3xF3iRy9K6eSi3SErMMWHZJuoW0GzaAjEYjUzV/F7c0MHlzKcwAAAABJRU5ErkJggg==");
                UserEntity user = new()
                {
                    Email = "system@localhost",
                    UserName = "ChatInfo",
                    Image = image
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
