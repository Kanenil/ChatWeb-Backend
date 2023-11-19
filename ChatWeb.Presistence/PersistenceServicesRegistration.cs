using ChatWeb.Application.Contracts.Persistence;
using ChatWeb.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatWeb.Presistence;

public static class PersistenceServicesRegistration
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ChatConnectionString")));

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddTransient<IChatRepository, ChatRepository>();
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IMessagesRepository, MessagesRepository>();

        return services;
    }
}
