using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class UserServiceTests
{
    private readonly UserService userService;
    private readonly ApplicationDbContext dbContext;
    private readonly Mock<IMapper> mockMapper;

    public UserServiceTests()
    {
        // In-memory database setup
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
            .Options;
        dbContext = new ApplicationDbContext(options);

        mockMapper = new Mock<IMapper>();

        userService = new UserService(dbContext, mockMapper.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        dbContext.Users.Add(new User { Id = 1, Name = "John Doe" });
        dbContext.Users.Add(new User { Id = 2, Name = "Jane Smith" });
        await dbContext.SaveChangesAsync();

        var userDtos = new List<UserDto>
        {
            new UserDto { Id = 1, Name = "John Doe" },
            new UserDto { Id = 2, Name = "Jane Smith" }
        };
        mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<List<User>>())).Returns(userDtos);

        var result = await userService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User { Id = 1, Name = "John Doe" };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var userDto = new UserDto { Id = 1, Name = "John Doe" };
        mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(userDto);

        var result = await userService.GetUserByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnDefaultUser_WhenUserDoesNotExist()
    {
        var defaultUserDto = new UserDto();
        mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(defaultUserDto);

        var result = await userService.GetUserByIdAsync(99);

        Assert.NotNull(result);
        Assert.Equal(0, result.Id); 
    }
}