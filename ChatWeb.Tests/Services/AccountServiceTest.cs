using AutoMapper;
using ChatWeb.Application.Profiles;
using ChatWeb.Domain.Identity;
using ChatWeb.Infrastructure.Identity;
using ChatWeb.Presistence.Repositories;

namespace ChatWeb.Tests.Services;

[TestClass]
public class AccountServiceTest
{
    [TestMethod]
    public async Task Profile_ShouldReturnUserDTO()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        dbContext.Database.EnsureDeleted();

        UserEntity user = new()
        {
            UserName = "Test",
            Email = "test@gmail.com"
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var usersRepository = new UsersRepository(dbContext);
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UsersProfile()); 
        });
        var mapper = mockMapper.CreateMapper();
        var accountService = new AccountService(usersRepository, mapper);

        // Act
        var result = await accountService.Profile(user.UserName);

        // Assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task Logout_ShouldClearData()
    {
        // Arrange
        using var dbContext = ChatDbContextFactory.CreateDbContext();
        dbContext.Database.EnsureDeleted();

        UserEntity user = new()
        {
            UserName = "Test",
            Email = "test@gmail.com",
            RefreshToken = Path.GetRandomFileName(),
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var usersRepository = new UsersRepository(dbContext);
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UsersProfile());
        });
        var mapper = mockMapper.CreateMapper();
        var accountService = new AccountService(usersRepository, mapper);

        // Act
        await accountService.Logout(user.UserName);

        // Assert
        Assert.IsNull(user.RefreshToken);
    }
}
