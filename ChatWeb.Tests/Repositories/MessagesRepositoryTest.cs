using ChatWeb.Domain;
using ChatWeb.Domain.Identity;
using ChatWeb.Presistence.Repositories;

namespace ChatWeb.Tests.Repositories;

[TestClass]
public class MessagesRepositoryTest
{
    [TestMethod]
    public async Task GetAsync_ShouldEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var messageRepository = new MessagesRepository(dbContext);

        UserEntity user = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        MessageEntity message = new()
        {
            Content = "Test message",
            UserId = user.Id,
        };

        await dbContext.AddAsync(message);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await messageRepository.GetAsync(message.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(message.Id, result.Id);
    }

    [TestMethod]
    public async Task GetAllByChatIdAsync_ShouldCount()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var messageRepository = new MessagesRepository(dbContext);

        ChatEntity chat = new()
        {
            Name = "Test",
        };

        UserEntity user = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        await dbContext.AddAsync(user);
        await dbContext.AddAsync(chat);
        await dbContext.SaveChangesAsync();

        MessageEntity message1 = new()
        {
            Content = "Test message",
            UserId = user.Id,
            ChatId = chat.Id
        };

        MessageEntity message2 = new()
        {
            Content = "Test message2",
            UserId = user.Id,
            ChatId = chat.Id
        };

        await dbContext.AddAsync(message1);
        await dbContext.AddAsync(message2);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await messageRepository.GetAllByChatIdAsync(chat.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }
}
