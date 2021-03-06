﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Field
	{
		[Display(Name = "شناسه")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Id { get; set; }

		[Display(Name = "گروه فیلد")]
		public string FieldGroupId { get; set; }

		[Display(Name = "گروه فیلد")]
		public virtual FieldGroup FieldGroup { get; set; }

		[Display(Name = "نوع فیلد")]
		public string FieldTypeId { get; set; }

		[Display(Name = "نوع فیلد")]
		public FieldType FieldType { get; set; }

		[Display(Name = "عنوان")]
		public string Title { get; set; }

        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [NotMapped]
        [Display(Name = "دسته بندی محصولات")]
        public string[] CategoryIds { get; set; }
        public ICollection<CategoryField> CategoryFields { get; set; }

		public ICollection<ProductField> ProductFields { get; set; }
	}
}