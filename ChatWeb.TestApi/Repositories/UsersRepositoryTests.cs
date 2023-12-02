using ChatWeb.Domain.Identity;
using ChatWeb.Presistence.Repositories;
using ChatWeb.Presistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;

namespace ChatWeb.TestApi.Repositories;

public class UsersRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsersWithMessagesAndChatGroups()
    {
        // Arrange
        var dbContextMock = new Mock<ChatDbContext>();
        var usersRepository = new UsersRepository(dbContextMock.Object);

        // Mock the DbSet<UserEntity>
        var users = new List<UserEntity>
        {
            // Add some test data
        }.AsQueryable();

        var dbSetMock = new Mock<DbSet<UserEntity>>();
        dbSetMock.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

        dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);

        // Act
        var result = await usersRepository.GetAllAsync();

        // Assert
        Assert.Equal(users.Count(), result.Count);
        // Add more specific assertions based on your data
    }

    // Add more test methods for other repository methods

    // ...

    [Fact]
    public async Task GenericRepository_AddAsync_ShouldAddEntityToDbContext()
    {
        // Arrange
        var dbContextMock = new Mock<ChatDbContext>();
        var genericRepository = new GenericRepository<UserEntity>(dbContextMock.Object);
        var entityToAdd = new UserEntity();

        var dbSetMock = new Mock<DbSet<UserEntity>>();
        dbContextMock.Setup(x => x.Set<UserEntity>()).Returns(dbSetMock.Object);

        // Act
        await genericRepository.AddAsync(entityToAdd);

        // Assert
        dbSetMock.Verify(x => x.AddAsync(entityToAdd, It.IsAny<CancellationToken>()), Times.Once);
        dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
