using AutoMapper;
using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly IMapper mapper;

        public CategoryRepository(LuftbornContext context,IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public async Task<ResponseDto> AddAsync(CategoryDto entity)
        {
            try
            {
                ResponseDto res = new();
                Category category = mapper.Map<Category>(entity);
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                res.Body = mapper.Map<CategoryDto>(category);
                res.Message = "category added sucessfully";
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public new async Task<ResponseDto> GetAllAsync()
        {
            try
            {
                ResponseDto res = new();
                List<Category> categories = await context.Categories.ToListAsync();
                res.Body = mapper.Map<List<CategoryDto>>(categories);
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                ResponseDto res = new();
                Category? category = await context.Categories.FirstOrDefaultAsync(w => w.Id == id);
                if (category is null)
                {
                    res.Message = "welder not found.";
                    return res;
                }
                res.Body = mapper.Map<CategoryDto>(category);
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> Remove(Guid id)
        {
            try
            {
                ResponseDto res = new();
                Category? category = await context.Categories.FirstOrDefaultAsync(w => w.Id == id);
                if (category is null)
                {
                    res.Message = "category not found.";
                    return res;
                }
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                res.Message = "category deleted sucessfully";
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> Update(CategoryDto entity)
        {
            try
            {
                ResponseDto res = new();
                if (!await context.Categories.AsNoTracking().AnyAsync(w => w.Id == entity.Id))
                {
                    res.Message = "category not found.";
                    return res;
                }
                Category category = mapper.Map<Category>(entity);
                context.Categories.Update(category);
                await context.SaveChangesAsync();
                res.Body = mapper.Map<CategoryDto>(category);
                res.Message = "category updated sucessfully";
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> DeleteItemsAsync(List<Guid> Ids)
        {
            try
            {
                ResponseDto res = new();
                foreach (Guid Id in Ids)
                {
                    var item = await context.Items.FirstOrDefaultAsync(w => w.Id == Id);
                    if (item == null) continue;
                    context.Remove(item);
                };
                await context.SaveChangesAsync();
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }
    }
}
