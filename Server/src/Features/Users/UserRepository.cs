using src.Database;
using src.Features.Shared.Interfaces;

namespace src.Features.Users;

public class UserRepository(StudentBlogDbContext dbContext) : IBaseRepository<User>
{
    public async Task<User?> AddAsync(User user)
    {
        await dbContext.User.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}