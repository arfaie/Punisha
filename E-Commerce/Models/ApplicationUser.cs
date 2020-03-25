using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

		[Display(Name = "خودرو")]
		public string CarId { get; set; }

		[Display(Name = "خودرو")]
		public virtual Car Car { get; set; }

		[Display(Name = "گروه کاربری")]
		public string UserGroupId { get; set; }

		[Display(Name = "گروه کاربری")]
		public virtual UserGroup UserGroup { get; set; }

		public ICollection<Address> Addresses { get; set; }

		public ICollection<Factor> Factors { get; set; }

		public virtual ICollection<ApplicationRole> ApplicationRoles { get; set; }

		public DateTime RegistrationDateAndTime { get; set; }

		public bool IsBlocked { get; set; }
	}
}