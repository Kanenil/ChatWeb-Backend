using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChatWeb.Presistence;

public class ChatDbContextFactory :
        IDesignTimeDbContextFactory<ChatDbContext>
{
    public ChatDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

        var builder = new DbContextOptionsBuilder<ChatDbContext>();
        var connectionString = configuration.GetConnectionString("ChatConnectionString");

        builder.UseSqlServer(connectionString);

        return new ChatDbContext(builder.Options);
    }
}