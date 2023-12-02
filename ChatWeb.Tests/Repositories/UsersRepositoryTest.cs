using ChatWeb.Domain.Identity;
using ChatWeb.Domain;
using ChatWeb.Presistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWeb.Presistence;

namespace ChatWeb.Tests.Repositories;

[TestClass]
public class UsersRepositoryTest
{

    [TestMethod]
    public async Task GetAllAsync_ShouldEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        dbContext.Database.EnsureDeleted();
        var userRepository = new UsersRepository(dbContext);

        UserEntity user1 = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        UserEntity user2 = new()
        {
            UserName = "Test2",
            Email = "test2@gmail.com"
        };

        UserEntity user3 = new()
        {
            UserName = "Test3",
            Email = "test3@gmail.com"
        };

        await dbContext.AddRangeAsync(user1, user2, user3);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Count);
    }

    [TestMethod]
    public async Task GetUsersByUsernameSearchAsync_ShouldCount()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        dbContext.Database.EnsureDeleted();
        var userRepository = new UsersRepository(dbContext);

        UserEntity user1 = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        UserEntity user2 = new()
        {
            UserName = "Test2",
            Email = "test2@gmail.com"
        };

        UserEntity user3 = new()
        {
            UserName = "User3",
            Email = "test3@gmail.com"
        };

        await dbContext.AddRangeAsync(user1, user2, user3);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetUsersByUsernameSearchAsync("Tes", user3.UserName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public async Task GetUsersByUsernameAsync_ShouldEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new UsersRepository(dbContext);

        UserEntity user1 = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        UserEntity user2 = new()
        {
            UserName = "Test2",
            Email = "test2@gmail.com"
        };

        UserEntity user3 = new()
        {
            UserName = "User3",
            Email = "test3@gmail.com"
        };

        await dbContext.AddRangeAsync(user1, user2, user3);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetUserByUsernameAsync("User3");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("User3", result.UserName);
    }

    [TestMethod]
    public async Task GetAsync_ShouldEquals()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        var userRepository = new UsersRepository(dbContext);

        UserEntity user1 = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        UserEntity user2 = new()
        {
            UserName = "Test2",
            Email = "test2@gmail.com"
        };

        UserEntity user3 = new()
        {
            UserName = "User3",
            Email = "test3@gmail.com"
        };

        await dbContext.AddRangeAsync(user1, user2, user3);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await userRepository.GetAsync(user2.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user2.UserName, result.UserName);
    }
}
