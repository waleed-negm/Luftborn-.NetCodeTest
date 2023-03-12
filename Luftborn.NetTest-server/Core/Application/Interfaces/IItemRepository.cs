
using Core.Application.Dto;
using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        new Task<ResponseDto> GetAllAsync();

        new Task<ResponseDto> GetByIdAsync(Guid id);

        Task<ResponseDto> AddAsync(ItemDto entity);

        Task<ResponseDto> Update(ItemDto entity);

        Task<ResponseDto> Remove(Guid id);
    }
}
