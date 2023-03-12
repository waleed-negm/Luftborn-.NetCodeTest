using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

    }
}
