using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Repositories;
using Moq;
using Xunit;

public class UserRepositoryTests        
{
          private readonly ApplicationDbContext dbContext;
          private readonly UserRepository repository;
          public UserRepositoryTests()
          {
                    // Initialize the in-memory database for testing
                    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                              .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                              .Options;
                    dbContext = new ApplicationDbContext(options);

                    // Seed data
                    dbContext.Users.Add(new User { Id = 1, Name = "John Doe" });
                    dbContext.Users.Add(new User { Id = 2, Name = "Jane Smith" });
                    dbContext.SaveChanges();

                    repository = new UserRepository(dbContext);
          }

          [Fact]
          public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
          {
                    var userId = 1;
                    var user = await repository.GetUserByIdAsync(userId);
                    Assert.NotNull(user);
                    Assert.Equal(userId, user.Id);
          }
          [Fact]
          public async Task GetUserByIdAsync_ShouldThrowKeyNotFoundException_WhenUserDoesNotExist()
          {
                    var userId = 10; // Non-existing user ID
                    await Assert.ThrowsAsync<KeyNotFoundException>(async () => await repository.GetUserByIdAsync(userId));
          }
          [Fact]
          public async Task GetAllUsersAsync_ShouldReturnAllUsers()
          {
                    var users = await repository.GetAllUsersAsync();
                    Assert.NotNull(users);
                    Assert.Equal(2, users.Count());
          }
}