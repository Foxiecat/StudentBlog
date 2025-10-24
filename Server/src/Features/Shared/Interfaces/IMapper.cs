namespace src.Features.Shared.Interfaces;

public interface IMapper<in TRequest, out TResponse, TEntity>
{
    TEntity ToEntity(TRequest request);
    TResponse ToResponse(TEntity entity);
}