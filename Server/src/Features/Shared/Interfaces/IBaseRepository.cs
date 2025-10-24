using src.Features.Users;

namespace src.Features.Shared.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> AddAsync(TEntity entity);
}