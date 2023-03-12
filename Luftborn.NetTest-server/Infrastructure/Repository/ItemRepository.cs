using AutoMapper;
using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly IMapper mapper;

        public ItemRepository(LuftbornContext context,IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }
        public async Task<ResponseDto> AddAsync(ItemDto entity)
        {
            try
            {
                ResponseDto res = new();
                Item item = mapper.Map<Item>(entity);
                context.Categories.FindAsync(item.CategoryId).Result?.Items.Add(item);
                await context.SaveChangesAsync();
                res.Body = mapper.Map<ItemDto>(item);
                res.Message = "item added sucessfully";
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
                List<Item> items = await context.Items.ToListAsync();
                res.Body = mapper.Map<List<ItemDto>>(items);
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
                Item? item = await context.Items.FirstOrDefaultAsync(w => w.Id == id);
                if (item is null)
                {
                    res.Message = "item not found.";
                    return res;
                }
                res.Body = mapper.Map<ItemDto>(item);
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
                Item? item = await context.Items.FirstOrDefaultAsync(w => w.Id == id);
                if (item is null)
                {
                    res.Message = "item not found.";
                    return res;
                }
                context.Items.Remove(item);
                await context.SaveChangesAsync();
                res.Message = "item deleted sucessfully";
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }

        public async Task<ResponseDto> Update(ItemDto entity)
        {
            try
            {
                ResponseDto res = new();
                if (!await context.Items.AsNoTracking().AnyAsync(w => w.Id == entity.Id))
                {
                    res.Message = "item not found.";
                    return res;
                }
                Item item = mapper.Map<Item>(entity);
                context.Items.Update(item);
                await context.SaveChangesAsync();
                res.Body = mapper.Map<ItemDto>(item);
                res.Message = "item updated sucessfully";
                res.Status = true;
                return res;
            }
            catch (Exception ex) { return new ResponseDto() { Message = ex.Message, Body = ex }; }
        }
    }
}