using ChatWeb.Domain;
using ChatWeb.Domain.Identity;
using ChatWeb.Presistence.Repositories;

namespace ChatWeb.Tests.Repositories;

[TestClass]
public class ChatRepositoryTest
{
    private List<ChatEntity> Chats()
    {
        return new List<ChatEntity>() 
        { 
            new ChatEntity() { Name = "Test 1" }, 
            new ChatEntity() { Name = "Test 2" }, 
            new ChatEntity() { Name = "Test 3" }, 
        };
    }

    private List<ChatGroupEntity> GroupsWithUserId(List<ChatEntity> chats, int userId)
    {
        var list = new List<ChatGroupEntity>();

        foreach (ChatEntity ch in chats)
        {
            list.Add(new ChatGroupEntity() { ChatId = ch.Id, UserId = userId });
        }

        return list;
    }

    [TestMethod]
    public async Task GetAsync_ShouldEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new ChatRepository(dbContext);

        ChatEntity chatEntity = new() 
        { 
            Name = "Test",
        };

        await dbContext.AddAsync(chatEntity);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetAsync(chatEntity.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(chatEntity.Name, result.Name);
    }

    [TestMethod]
    public async Task AddChatGroupAsync_ShouldReturnChatEntity()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new ChatRepository(dbContext);

        ChatEntity chatEntity = new()
        {
            Name = "Test",
        };

        UserEntity userEntity = new()
        {
            UserName ="Test",
            Email = "test@gmail.com"
        };

        await dbContext.AddAsync(chatEntity);
        await dbContext.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();

        ChatGroupEntity chatGroupEntity = new()
        {
            ChatId = chatEntity.Id,
            UserId = userEntity.Id
        };

        // Act
        var result = await userRepository.AddChatGroupAsync(chatGroupEntity);
        await userRepository.SaveAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(chatGroupEntity, result);
        Assert.AreEqual(1, chatEntity.ChatGroups.Count);
    }

    [TestMethod]
    public async Task GetAllByUserIdAsync_ShouldReturnList()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new ChatRepository(dbContext);

        UserEntity userEntity = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        var chats = Chats();

        await dbContext.AddAsync(userEntity);
        await dbContext.AddRangeAsync(chats);
        await dbContext.AddRangeAsync(GroupsWithUserId(chats, userEntity.Id));
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetAllByUserIdAsync(userEntity.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count());
    }

    [TestMethod]
    public async Task GetByUserIdAsync_ShouldReturnChatEntity()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new ChatRepository(dbContext);

        UserEntity userEntity = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        var chats = Chats();

        await dbContext.AddAsync(userEntity);
        await dbContext.AddRangeAsync(chats);
        await dbContext.AddRangeAsync(GroupsWithUserId(chats, userEntity.Id));
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetByUserIdAsync(chats[1].Id, userEntity.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test 2", result.Name);
    }

    [TestMethod]
    public async Task GetByUserIdAsync_ShouldNotEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new ChatRepository(dbContext);

        UserEntity userEntity = new()
        {
            UserName = "Test1",
            Email = "test1@gmail.com"
        };

        UserEntity userEntity2 = new()
        {
            UserName = "Test2",
            Email = "test2@gmail.com"
        };

        var chats = Chats();

        await dbContext.AddAsync(userEntity);
        await dbContext.AddAsync(userEntity2);
        await dbContext.AddRangeAsync(chats);
        await dbContext.AddRangeAsync(GroupsWithUserId(chats, userEntity.Id));
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetByUserIdAsync(2, userEntity2.Id);

        // Assert
        Assert.IsNull(result);
    }
}
