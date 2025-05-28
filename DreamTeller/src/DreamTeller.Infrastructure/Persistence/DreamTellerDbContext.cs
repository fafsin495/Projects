using Microsoft.EntityFrameworkCore;
using DreamTeller.Domain.Entities;

namespace DreamTeller.Infrastructure.Persistence
{
    public class DreamTellerDbContext : DbContext
    {
        public DreamTellerDbContext(DbContextOptions<DreamTellerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SubscriptionTransaction> SubscriptionTransactions { get; set; }
        public DbSet<OutboxEmail> OutboxEmails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users");
                builder.HasKey(u => u.Id);
                builder.Property(u => u.Id).ValueGeneratedOnAdd(); // Assuming UUID generation is handled by DB default
                builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
                builder.HasIndex(u => u.Email).IsUnique();
                builder.Property(u => u.PasswordHash).IsRequired(false); // TEXT can be null
                builder.Property(u => u.FullName).HasMaxLength(256).IsRequired(false);
                builder.Property(u => u.Gender).IsRequired(false);
                builder.Property(u => u.IsVip).HasDefaultValue(false);
                builder.Property(u => u.SubscriptionEndDate).IsRequired(false);
                builder.Property(u => u.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"); // Assuming DB handles default
                builder.Property(u => u.UpdatedOn).IsRequired(false);
            });

            // Configure UserVerification entity
            modelBuilder.Entity<UserVerification>(builder =>
            {
                builder.ToTable("UserVerifications");
                builder.HasKey(uv => uv.Id);
                builder.Property(uv => uv.Id).ValueGeneratedOnAdd(); // Assuming UUID generation is handled by DB default
                builder.Property(uv => uv.UserId).IsRequired();
                builder.Property(uv => uv.Code).IsRequired().HasMaxLength(10);
                builder.Property(uv => uv.Type).IsRequired();
                builder.Property(uv => uv.ExpireAt).IsRequired();
                builder.Property(uv => uv.IsUsed).HasDefaultValue(false);
                builder.Property(uv => uv.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"); // Assuming DB handles default

                builder.HasOne(uv => uv.User)
                       .WithMany()
                       .HasForeignKey(uv => uv.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure RefreshToken entity
            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.ToTable("RefreshTokens");
                builder.HasKey(rt => rt.Id);
                builder.Property(rt => rt.Id).ValueGeneratedOnAdd(); // Assuming UUID generation is handled by DB default
                builder.Property(rt => rt.UserId).IsRequired();
                builder.Property(rt => rt.Token).IsRequired(); // TEXT can be not null
                builder.Property(rt => rt.ExpiryDate).IsRequired();
                builder.Property(rt => rt.IsRevoked).HasDefaultValue(false);
                builder.Property(rt => rt.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"); // Assuming DB handles default

                builder.HasOne(rt => rt.User)
                       .WithMany()
                       .HasForeignKey(rt => rt.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SubscriptionTransaction entity
            modelBuilder.Entity<SubscriptionTransaction>(builder =>
            {
                builder.ToTable("SubscriptionTransactions");
                builder.HasKey(st => st.Id);
                builder.Property(st => st.Id).ValueGeneratedOnAdd(); // Assuming UUID generation is handled by DB default
                builder.Property(st => st.UserId).IsRequired();
                builder.Property(st => st.StoreType).IsRequired();
                builder.Property(st => st.ExternalTransactionId).IsRequired(); // TEXT can be not null
                builder.Property(st => st.PeriodMonths).IsRequired();
                builder.Property(st => st.IsValid).IsRequired();
                builder.Property(st => st.CheckedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"); // Assuming DB handles default

                builder.HasOne(st => st.User)
                       .WithMany()
                       .HasForeignKey(st => st.UserId)
                       .OnDelete(DeleteBehavior.ClientSetNull); // Assuming no cascade delete based on schema
            });

            // Configure OutboxEmail entity
            modelBuilder.Entity<OutboxEmail>(builder =>
            {
                builder.ToTable("OutboxEmails");
                builder.HasKey(oe => oe.Id);
                builder.Property(oe => oe.Id).ValueGeneratedOnAdd(); // Assuming UUID generation is handled by DB default
                builder.Property(oe => oe.ToEmail).IsRequired().HasMaxLength(256);
                builder.Property(oe => oe.Subject).HasMaxLength(256).IsRequired(false);
                builder.Property(oe => oe.Body).IsRequired(false); // TEXT can be null
                builder.Property(oe => oe.IsSent).HasDefaultValue(false);
                builder.Property(oe => oe.SendTries).HasDefaultValue(0);
                builder.Property(oe => oe.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"); // Assuming DB handles default
            });
        }
    }
} 