using ECommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Agency> Agencies { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CategoryGroup> CategoryGroups { get; set; }
		public DbSet<CategoryField> CategoryFields { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<CommentAndStar> CommentAndStars { get; set; }
		public DbSet<Factor> Factors { get; set; }
		public DbSet<FactorItem> FactorItems { get; set; }
		public DbSet<Field> Fields { get; set; }
		public DbSet<FieldGroup> FieldGroups { get; set; }
		public DbSet<FieldType> FieldTypes { get; set; }
		public DbSet<History> Histories { get; set; }
		public DbSet<InventoryChange> InventoryChanges { get; set; }
		public DbSet<Maker> Makers { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<OfferItem> OfferItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<PriceChange> PriceChanges { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductField> ProductFields { get; set; }
		public DbSet<ProductGallery> ProductGalleries { get; set; }
		public DbSet<ProductSelectedItem> ProductSelectedItems { get; set; }
		public DbSet<SelectGroup> SelectGroups { get; set; }
		public DbSet<SelectItem> SelectItems { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			builder.Entity<Address>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Agency>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Brand>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Car>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Category>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<CategoryGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<CategoryField>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<City>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<CommentAndStar>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Factor>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<FactorItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<FieldGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<FieldType>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<History>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<InventoryChange>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Maker>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Offer>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<OfferItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Order>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<PriceChange>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Product>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<ProductField>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<ProductGallery>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<ProductSelectedItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<SelectGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<SelectItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Slider>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<State>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Status>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Unit>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<UserGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
		}
	}
}