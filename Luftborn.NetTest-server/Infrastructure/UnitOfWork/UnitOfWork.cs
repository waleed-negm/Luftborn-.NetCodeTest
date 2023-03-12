using AutoMapper;
using Core.Application.Interfaces;
using Core.Application.IunitOfWork;
using Infrastructure.Context;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LuftbornContext context;
        private readonly IMapper mapper;

        public ICategoryRepository Categories { get; private set; }
        public IItemRepository Items { get; private set; }

        public UnitOfWork(LuftbornContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            Categories = new CategoryRepository(context, mapper);
            Items = new ItemRepository(context, mapper);
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
