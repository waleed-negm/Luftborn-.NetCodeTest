using Core.Domain.LKP;
using System.ComponentModel.DataAnnotations;
namespace Core.Domain.Entities
{
    public class Category : BaseModel
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        public virtual List<Item>? Items { get; set; }
    }
}
