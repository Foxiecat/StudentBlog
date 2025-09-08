using System.Linq.Expressions;
using src.Entities;
using src.Features.Shared.Interfaces;

namespace src.Features.Users;

public interface IUserRepository : IRepository<User>;
public class UserRepository : IUserRepository
{
    public async Task<User> AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<User> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task<User> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}