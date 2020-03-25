using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class FieldType
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "عنوان")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی باشد.")]
		public string Title { get; set; }

		public ICollection<Field> Fields { get; set; }
	}
}