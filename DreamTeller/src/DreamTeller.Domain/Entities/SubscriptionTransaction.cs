using System;

namespace DreamTeller.Domain.Entities
{
    public class SubscriptionTransaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public short StoreType { get; set; }
        public string ExternalTransactionId { get; set; } = string.Empty;
        public int PeriodMonths { get; set; }
        public bool IsValid { get; set; }
        public DateTime CheckedOn { get; set; }
    }
} 