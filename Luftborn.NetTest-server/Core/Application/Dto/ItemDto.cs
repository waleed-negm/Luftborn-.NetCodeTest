namespace Core.Application.Dto
{
    public class ItemDto
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryId { get; set; }

    }
}
