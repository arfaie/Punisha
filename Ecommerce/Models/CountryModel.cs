using System.Collections.Generic;

namespace Ecommerce.Models
{
	public class FieldTypeModel
	{
		public List<Types> Types { get; set; }
		public string SelectedType { get; set; }
	}

	public class Types
	{
		public short Id { get; set; }
		public string Title { get; set; }
	}
}