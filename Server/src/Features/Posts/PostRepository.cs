using System.Linq.Expressions;
using src.Entities;
using src.Features.Shared.Interfaces;

namespace src.Features.Posts;

public class PostRepository : IRepository<Post>
{
    public async Task<Post> AddAsync(Post entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Post>> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> UpdateAsync(Post entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}