namespace Core.Application.Dto.auth
{
    public class UserDto
    {
        public string Id { get; set; }
        public int AccessFailedCount {get; set;}
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfimed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfimed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
