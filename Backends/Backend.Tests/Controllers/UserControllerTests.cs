using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Models;
using Backend.DTOs;
using Backend.Services;
using Backend.interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UserControllerTests
{
   private readonly Mock<IUserService> userServiceMock;
   private readonly UserController userController;

   public UserControllerTests()
   {
          userServiceMock = new Mock<IUserService>();
          userController = new UserController(userServiceMock.Object);
   }

   [Fact]
   public async Task GetUserById_ShouldReturnOk_WhenUserExists()
   {
          var userId = 1;
          var userDto = new UserDto { Id = userId, Name = "John Doe" };
          userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

          var result = await userController.GetUserById(userId);

          var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
          var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
          var returnedUser = Assert.IsType<UserDto>(okResult.Value);
          Assert.Equal(userId, returnedUser.Id);
   }

   [Fact]
   public async Task CreateUser_ShouldReturnBadRequest_WhenUserIsInvalid()
   {
          userController.ModelState.AddModelError("Name", "Required");
          var userDto = new UserDto { Id= 1, Name = "" };
          var result = await userController.CreateUser(userDto);
          var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
          Assert.IsType<BadRequestObjectResult>(actionResult.Result);
   }

   [Fact]
   public async Task DeleteUser_ShouleReturnNoContent_WhenUserIsDeleted()
   {
          var userId = 1;
          userServiceMock.Setup(s => s.DeleteUserAsync(It.Is<int>(id => id == userId))).Returns(Task.CompletedTask);
          var result = await userController.DeleteUser(userId);
          Assert.IsType<NoContentResult>(result);
   }

   [Fact]
   public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist() 
   {
         var userId = 1;
         userServiceMock.Setup(s => s.DeleteUserAsync(It.Is<int>(id => id == userId))).ThrowsAsync(new KeyNotFoundException("User not found"));
         var result = await userController.DeleteUser(userId); 
         Assert.IsType<NotFoundResult>(result);
   }
}