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
		public DbSet<CarProduct> CarProducts { get; set; }
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
		public DbSet<SelectGroup> SelectGroups { get; set; }
		public DbSet<SelectItem> SelectItems { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<UserGroup> UserGroups { get; set; }

		public DbSet<ShippingState> ShippingStates { get; set; }

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
			builder.Entity<CarProduct>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
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
			builder.Entity<SelectGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<SelectItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Slider>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<State>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Status>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<Unit>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
			builder.Entity<UserGroup>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

			builder.Entity<ShippingState>().HasData(
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec01", StateId = 1, Name = "تهران" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec02", StateId = 2, Name = "گیلان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec03", StateId = 3, Name = "آذربایجان شرقی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec04", StateId = 4, Name = "خوزستان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec05", StateId = 5, Name = "فارس" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec06", StateId = 6, Name = "اصفهان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec07", StateId = 7, Name = "خراسان رضوی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec08", StateId = 8, Name = "قزوین" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec09", StateId = 9, Name = "سمنان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec10", StateId = 10, Name = "قم" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec11", StateId = 11, Name = "مرکزی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec12", StateId = 12, Name = "زنجان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec13", StateId = 13, Name = "مازندران" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec14", StateId = 14, Name = "گلستان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec15", StateId = 15, Name = "اردبیل" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec16", StateId = 16, Name = "آذربایجان غربی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec17", StateId = 17, Name = "همدان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec18", StateId = 18, Name = "کردستان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec19", StateId = 19, Name = "کرمانشاه" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec20", StateId = 20, Name = "لرستان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec21", StateId = 21, Name = "بوشهر" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec22", StateId = 22, Name = "کرمان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec23", StateId = 23, Name = "هرمزگان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec24", StateId = 24, Name = "چهارمحال و بختیاری" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec25", StateId = 25, Name = "یزد" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec26", StateId = 26, Name = "سیستان و بلوچستان" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec27", StateId = 27, Name = "ایلام" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec28", StateId = 28, Name = "کهگلویه و بویراحمد" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec29", StateId = 29, Name = "خراسان شمالی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec30", StateId = 30, Name = "خراسان جنوبی" },
				new ShippingState { Id = "6f9c65d6-6596-4fd0-8193-7c32dafcec31", StateId = 31, Name = "البرز" }
			);
		}
	}
}