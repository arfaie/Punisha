using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Display(Name = "نام و نام خانوادگی")]
		public string FullName { get; set; }

		[Display(Name = "کد ملی")]
		public long NationalCode { get; set; }

		[Display(Name = "کد تاکسی")]
		public string TaxiCode { get; set; }

		[Display(Name = "مدل خودرو")]
		public string CarId { get; set; }

		[Display(Name = "مدل خودرو")]
		public virtual Car Car { get; set; }

		[Display(Name = "گروه کاربری")]
		public string UserGroupId { get; set; }

		[Display(Name = "گروه کاربری")]
		public virtual UserGroup UserGroup { get; set; }

		[Display(Name = "زمان ثبت نام")]
		public DateTime RegistrationDateAndTime { get; set; }

		[Display(Name = "مسدود")]
		public bool IsBlocked { get; set; }

		[NotMapped]
		[Display(Name = "نقش")]
		public string ApplicationRoleId { get; set; }

		[NotMapped]
		[Display(Name = "رمز عبور")]
		public string Password { get; set; }

		public ICollection<Address> Addresses { get; set; }

		public ICollection<Factor> Factors { get; set; }
	}
}