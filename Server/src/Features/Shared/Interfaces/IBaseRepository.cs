using System.Linq.Expressions;

namespace src.Features.Shared.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> UpdateAsync(TEntity entity);
    Task<TEntity?> DeleteByIdAsync(Guid id);
}