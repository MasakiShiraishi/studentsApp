using System;
using System.Collections.Generic;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            // var user = new User
            var users = new List<User>
            {
               new User
            {
                Name = "John Doe",
                Phone = "1234567890",
                Email = "john.doe@example.com",
                Password = HashPassword("password"),
                Role = "Student",
                Class = "3rd Grade"
            },
               new User
            {
                Name = "Jane Smith",
                Phone = "0987654321",
                Email = "jane.smith@example.com",
                Password = HashPassword("password"),
                Role = "Teacher",
                Class = "Math"
             },
            };
            if(!await context.Users.AnyAsync())
            {
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }            
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during database seeding: {ex.Message}");
        }
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password); // Use a secure hashing algorithm
    }
}
