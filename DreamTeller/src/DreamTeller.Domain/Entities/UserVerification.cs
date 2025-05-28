namespace DreamTeller.Domain.Entities
{
    public class UserVerification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public string Code { get; set; } = string.Empty;
        public short Type { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedOn { get; set; }
    }
} 