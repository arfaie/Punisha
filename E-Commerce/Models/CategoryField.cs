using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class CategoryField
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "دسته بندی")]
		public string CategoryId { get; set; }

		[Display(Name = "دسته بندی")]
		public virtual Category Category { get; set; }

		[Display(Name = "قیلد")]
		public string FieldId { get; set; }

		[Display(Name = "قیلد")]
		public virtual Field Field { get; set; }
	}
}