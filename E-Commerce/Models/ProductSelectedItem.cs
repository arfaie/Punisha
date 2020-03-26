using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class ProductSelectedItem
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string ProductFieldId { get; set; }

		public virtual ProductField ProductField { get; set; }

		public string SelectItemId { get; set; }

		public virtual SelectItem SelectItem { get; set; }
	}
}