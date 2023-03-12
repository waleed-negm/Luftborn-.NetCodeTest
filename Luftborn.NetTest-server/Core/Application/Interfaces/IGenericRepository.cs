namespace Core.Application.Interfaces
{
    public interface IGenericRepository<T> where T:class
    {
        Task<List<T>> GetAllAsync();
        
        Task<T?> GetByIdAsync(Guid id);
        
        Task AddAsync(T entity);
        
        void Update (T entity);
        
        void Remove(T entity);

    }
}
