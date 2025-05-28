using System;

namespace DreamTeller.Domain.Entities
{
    public class OutboxEmail
    {
        public Guid Id { get; set; }
        public string ToEmail { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsSent { get; set; }
        public int SendTries { get; set; }
        public DateTime CreatedOn { get; set; }
    }
} 