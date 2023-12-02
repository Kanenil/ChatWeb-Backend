using ChatWeb.Presistence;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.Tests;

public class ChatDbContextFactory
{
    public static ChatDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDb")
            .Options;

        return new ChatDbContext(options);
    }
}
