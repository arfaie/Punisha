using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class SelectItem
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		public string SelectGroupId { get; set; }

		public virtual SelectGroup SelectGroup { get; set; }

		[Display(Name = "عنوان")]
		public string Title { get; set; }

		public ICollection<ProductSelectedItem> ProductSelectedItems { get; set; }
	}
}