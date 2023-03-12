using Core.Application.Interfaces;

namespace Core.Application.IunitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Items { get; }
        ICategoryRepository Categories { get; }
        Task<int> CompleteAsync();
    }
}
