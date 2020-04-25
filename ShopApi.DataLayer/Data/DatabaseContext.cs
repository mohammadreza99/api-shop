using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Product;
using ShopApi.Domain.User;

namespace ShopApi.DataLayer.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExtraFieldCategory>().HasKey(sc => new { sc.ExtraFieldId, sc.CategoryId });
            modelBuilder.Entity<ProductTag>().HasKey(sc => new { sc.TagId, sc.ProductId });
            modelBuilder.Entity<ProductExtraField>().HasKey(sc => new { sc.ExtraFieldId, sc.ProductId, sc.ExtraFieldValueId });
            modelBuilder.Entity<ProductFeature>().HasKey(sc => new
            {
                sc.FeatureId,
                sc.ProductId,
                sc.FeatureValueId
            });
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<TokenValue> TokenValue { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Feature> Feature { get; set; }
        public DbSet<FeatureValue> FeatureValue { get; set; }
        public DbSet<ExtraField> ExtraField { get; set; }
        public DbSet<ExtraFieldGroup> ExtraFieldGroup { get; set; }
        public DbSet<ExtraFieldValue> ExtraFieldValue { get; set; }
        public DbSet<ExtraFieldCategory> ExtraFieldCategory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductExtraField> ProductExtraField { get; set; }
        public DbSet<ProductFeature> ProductFeature { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Discount> Discount { get; set; }
    }
}
