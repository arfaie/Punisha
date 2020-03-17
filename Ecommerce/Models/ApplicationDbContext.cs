using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<Addresses> Addresses { get; set; }

        public DbSet<Agency> Agencies { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<category> Categories { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<InventoryChange> InventoryChanges { get; set; }

        public DbSet<PriceChange> PriceChanges { get; set; }

        public DbSet<ProductGallery> ProductGalleries { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<FactorItem> FactorItems { get; set; }

        public DbSet<Factor> Factors { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<OfferItem> OfferItems { get; set; }

        public DbSet<Slider> Sliders { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<SelectItem> SelectItems { get; set; }

        public DbSet<SelectGroup> SelectGroups { get; set; }

        public DbSet<ProductCategoryFields> ProductCategoryFields { get; set; }

        public DbSet<ProductField> ProductFields { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldGroup> FieldGroups { get; set; }

        public DbSet<Car> Cars { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<ProductSelectedItems> ProductSelectedItems { get; set; }



    }
}
