using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		[Display(Name = "مدل خودرو")]
		public string CarId { get; set; }

		[Display(Name = "مدل خودرو")]
		public virtual Car Car { get; set; }

		[Display(Name = "نام")]
		public string Name { get; set; }

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

		[Display(Name = "زمان اضافه کردن")]
		public DateTime AddingDateTime { get; set; }

		//[Display(Name = "قیمت قدیم")]
		//[Required(ErrorMessage = "فیلد قیمت نمی تواند خالی یاشد.")]
		//public int OldPrice { get; set; }

		[Display(Name = "عکس")]
		public string ImageName { get; set; }

		public ICollection<FactorItem> FactorItems { get; set; }

		public ICollection<InventoryChange> InventoryChanges { get; set; }

		public ICollection<OfferItem> OfferItems { get; set; }

		public ICollection<PriceChange> PriceChanges { get; set; }

		public ICollection<ProductField> ProductFields { get; set; }

		public ICollection<ProductGallery> ProductGalleries { get; set; }

        public ICollection<History> Histories { get; set; }
	}
}