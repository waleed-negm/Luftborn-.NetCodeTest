
namespace Core.Application.Dto
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int? ItemsCount { get; set; }

        public List<ItemDto>? Items { get; set;}
    }
}
