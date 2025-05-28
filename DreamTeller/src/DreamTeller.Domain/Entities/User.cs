namespace DreamTeller.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; }
        public string? FullName { get; set; }
        public short? Gender { get; set; }
        public bool IsVip { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
} 