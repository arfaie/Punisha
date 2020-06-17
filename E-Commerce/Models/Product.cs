using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerce.Models
{
	public class Product
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "دسته بندی")]
		public string CategoryId { get; set; }

		[Display(Name = "دسته بندی")]
		public virtual Category Category { get; set; }

		[Display(Name = "برند")]
		public string BrandId { get; set; }

		[Display(Name = "برند")]
		public virtual Brand Brand { get; set; }

		[Display(Name = "نام")]
		public string Name { get; set; }

		[Display(Name = "وزن به گرم")]
		public int Weight { get; set; }

		[Display(Name = "توضیحات")]
		public string Description { get; set; }

		[Display(Name = "کد")]
		public int Code { get; set; }

		[Display(Name = "واحد")]
		public string UnitId { get; set; }

		[Display(Name = "واحد")]
		public virtual Unit Unit { get; set; }

		[Display(Name = "موجودی")]
		public int Inventory { get; set; } //موجودی

		[Display(Name = " قابل فروش")]
		public bool IsSellable { get; set; } //قابل فروش

		[Display(Name = "نقطه سفارش")]
		public int OrderPoint { get; set; } //نقطه سفارش

		[Display(Name = "قیمت")]
		[Required(ErrorMessage = "فیلد قیمت نمی تواند خالی یاشد.")]
		public int Price { get; set; }

        [Display(Name = "نمایش")]
        public bool IsShow { get; set; }

		[Display(Name = "زمان اضافه کردن")]
		public DateTime AddingDateTime { get; set; }

		[NotMapped]
		public float DiscountAmount { get; set; }

		[NotMapped]
		public float DiscountPercent { get; set; }

		[NotMapped]
		public float PriceWithDiscount => (1 - DiscountPercent / 100) * Price - DiscountAmount;

		[Display(Name = "عکس")]
		public string ImageName { get; set; }

		[NotMapped]
		[Display(Name = "مدل خودروها")]
		public string[] CarIds { get; set; }

		public ICollection<FactorItem> FactorItems { get; set; }

		public ICollection<InventoryChange> InventoryChanges { get; set; }

		public ICollection<OfferItem> OfferItems { get; set; }

		public ICollection<PriceChange> PriceChanges { get; set; }

		public ICollection<ProductField> ProductFields { get; set; }

		public ICollection<ProductGallery> ProductGalleries { get; set; }

		public ICollection<CommentAndStar> CommentAndStars { get; set; }

		public ICollection<History> Histories { get; set; }
        [JsonIgnore]
        public ICollection<CarProduct> CarProducts { get; set; }
	}
}