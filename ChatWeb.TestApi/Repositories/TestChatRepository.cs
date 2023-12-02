using ChatWeb.Domain;
using ChatWeb.Domain.Identity;
using ChatWeb.Presistence;
using ChatWeb.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatWeb.TestApi.Repositories;

public class TestChatRepository : IDisposable
{
    protected readonly ChatDbContext _context;
    public TestChatRepository()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _context = new ChatDbContext(options);

        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEqualEntity()
    {
        /// Arrange
        ChatEntity chat = new ChatEntity()
        {
            Name = "Test",
            Image = ""
        };
        _context.Chats.Add(chat);
        _context.SaveChanges();

        var sut = new ChatRepository(_context);

        /// Act
        var result = await sut.GetAsync(chat.Id);

        /// Assert
        Assert.NotNull(result);
        Assert.Equal(chat.Id, result.Id);
    }

    [Fact]
    public async Task AddChatGroupAsync_ShouldAddChatGroupAndReturnIt()
    {
        // Arrange
        ChatEntity chat = new()
        {
            Name = "Test",
            Image = ""
        };
        UserEntity user = new()
        {
            UserName = "test",
            Email = "test@gmail.com"
        };
        _context.Chats.Add(chat);
        _context.Users.Add(user);
        _context.SaveChanges();

        var repository = new ChatRepository(_context);

        var chatGroup = new ChatGroupEntity() { ChatId = chat.Id, UserId = user.Id };

        // Act
        var result = await repository.AddChatGroupAsync(chatGroup);
        await repository.SaveAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatGroup, result);
        Assert.Equal(1, _context.ChatGroups.Count());
    }


    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
