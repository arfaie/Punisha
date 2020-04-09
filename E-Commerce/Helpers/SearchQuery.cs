namespace ECommerce.Helpers
{
	public class SearchQuery
	{
		public string CategoryGroupId { get; set; }

		public string CategoryId { get; set; }

		public string BrandId { get; set; }

		public string CarId { get; set; }

		public string MakerId { get; set; }

		public SortingQueryTypes SortingQuery { get; set; }
	}
}