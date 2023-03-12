
using Core.Application.Dto;
using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

        new Task<ResponseDto> GetAllAsync();

        new Task<ResponseDto> GetByIdAsync(Guid id);

        Task<ResponseDto> AddAsync(CategoryDto entity);

        Task<ResponseDto> Update(CategoryDto entity);

        Task<ResponseDto> Remove(Guid id);

        Task<ResponseDto> DeleteItemsAsync(List<Guid> Ids);
    }
}
