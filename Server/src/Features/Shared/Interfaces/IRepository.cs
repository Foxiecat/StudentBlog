using System.Linq.Expressions;
using src.Entities;

namespace src.Features.Shared.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
}