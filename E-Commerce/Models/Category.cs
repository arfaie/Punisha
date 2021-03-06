﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerce.Models
{
	public class Category
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "گروه دسته بندی")]
		public string CategoryGroupId { get; set; }

		[Display(Name = "گروه دسته بندی")]
		public virtual CategoryGroup CategoryGroup { get; set; }

		[Display(Name = "عنوان")]
		[Required(ErrorMessage = "فیلد {0} نمی تواند خالی باشد.")]
		public string Title { get; set; }

		public ICollection<CategoryField> CategoryFields { get; set; }


        
		public ICollection<Product> Products { get; set; }

		public ICollection<Field> Fields { get; set; }
	}
}