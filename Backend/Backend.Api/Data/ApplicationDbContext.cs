using Microsoft.EntityFrameworkCore;
using Backend.Api.Models;

namespace Backend.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Main Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyMedia> PropertyMedia { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        
        // Communication Entities
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        // Review & Analytics Entities
        public DbSet<PropertyReview> PropertyReviews { get; set; }
        public DbSet<PropertyView> PropertyViews { get; set; }
        public DbSet<SavedSearch> SavedSearches { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureMainEntities(modelBuilder);
            ConfigureCommunicationEntities(modelBuilder);
            ConfigureAnalyticsEntities(modelBuilder);
        }

        private void ConfigureMainEntities(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // PropertyType
            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.TypeId);
                entity.HasIndex(e => e.TypeName).IsUnique();
            });

            // Property
            // modelBuilder.Entity<Property>(entity =>
            // {
            //     entity.HasKey(e => e.PropertyId);
            //     entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            //     entity.Property(e => e.Area).HasColumnType("decimal(10,2)");
            //     entity.Property(e => e.Latitude).HasColumnType("decimal(10,8)");
            //     entity.Property(e => e.Longitude).HasColumnType("decimal(11,8)");
            //     entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                
            //     entity.HasIndex(e => e.Location);
            //     entity.HasIndex(e => e.Price);
            //     entity.HasIndex(e => new { e.Bedrooms, e.Bathrooms });
            // });

            // PropertyMedia
            modelBuilder.Entity<PropertyMedia>(entity =>
            {
                entity.HasKey(e => e.MediaId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => new { e.PropertyId, e.DisplayOrder });
            });

            // Favorite
            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => e.FavoriteId);
                entity.HasIndex(e => new { e.UserId, e.PropertyId }).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }

        private void ConfigureCommunicationEntities(ModelBuilder modelBuilder)
        {
            // Conversation
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.ConversationId);
                entity.HasIndex(e => new { e.PropertyId, e.BuyerUserId, e.SellerUserId }).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Message
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);
                entity.Property(e => e.SenderUserId).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.ConversationId);
                entity.HasIndex(e => new { e.SenderUserId, e.IsRead });
            });

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => new { e.UserId, e.IsRead });
                entity.HasIndex(e => e.NotificationId);
            });
        }

        private void ConfigureAnalyticsEntities(ModelBuilder modelBuilder)
        {
            // PropertyReview
            modelBuilder.Entity<PropertyReview>(entity =>
            {
                entity.HasKey(e => e.ReviewId);
                entity.HasIndex(e => new { e.PropertyId, e.ReviewerId }).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // PropertyView
            modelBuilder.Entity<PropertyView>(entity =>
            {
                entity.HasKey(e => e.ViewId);
                entity.Property(e => e.ViewedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.PropertyId);
                entity.HasIndex(e => e.ViewerId);
                entity.HasIndex(e => e.ViewedAt);
            });

            // SavedSearch
            modelBuilder.Entity<SavedSearch>(entity =>
            {
                entity.HasKey(e => e.SearchId);
                entity.Property(e => e.MinPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaxPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinArea).HasColumnType("decimal(10,2)");
                entity.Property(e => e.MaxArea).HasColumnType("decimal(10,2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.UserId);
            });
        }
    }
}