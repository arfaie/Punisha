using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class ProductField
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string ProductId { get; set; }

		public virtual Product Product { get; set; }

		public string FieldId { get; set; }

		public virtual Field Field { get; set; }

		public string Value { get; set; }
	}
}