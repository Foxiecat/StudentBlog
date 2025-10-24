using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using src.Database;
using src.Features.Posts;
using src.Features.Users.Interfaces;

namespace src.Features.Users;

public class UserRepository(
    StudentBlogDbContext dbContext,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User?> AddAsync(User entity)
    {
        bool checkIfUserExists = dbContext.User.Any(user => user.Id == entity.Id);
        if (checkIfUserExists)
        {
            logger.LogWarning("User with ID {UserId} already exists.", entity.Id);
            return null;
        }
        
        await dbContext.User.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await dbContext.User.FirstOrDefaultAsync(user => user.Id.Value == id);
    }
    
    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await dbContext.User
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<User?> UpdateAsync(User entity)
    {
        User? existingUser = await dbContext.User.FindAsync(entity.Id.Value);

        if (existingUser == null)
        {
            logger.LogWarning("Could not find user: {UserId}", entity.Id);
            return null;
        }
        
        existingUser.Firstname = entity.Firstname;
        existingUser.Lastname = entity.Lastname;
        existingUser.Username = entity.Username;
        existingUser.Email = entity.Email;
        existingUser.Updated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User?> DeleteByIdAsync(Guid id)
    {
        User? deletedUser = await dbContext.User.FindAsync(id);

        if (deletedUser is null)
        {
            logger.LogWarning("Could not find user: {UserId}, deletion aborted.", id);
            return null;
        }

        dbContext.User.Remove(deletedUser);
        await dbContext.SaveChangesAsync();
        
        return deletedUser;
    }
}