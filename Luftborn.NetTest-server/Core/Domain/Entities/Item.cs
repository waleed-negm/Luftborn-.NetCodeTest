using Core.Domain.LKP;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities
{
    public class Item : BaseModel
    {
        [Required, MaxLength(200)]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        [Required]
        public virtual Category Category { get; set; }
    }
}
