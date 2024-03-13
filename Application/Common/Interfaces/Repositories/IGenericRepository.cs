namespace Application.Common.Interfaces.Repositories;

public interface IGenericRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task CommitAsync();
}
