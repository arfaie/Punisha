using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ECommerce.Helpers
{
	public static class SessionExtensions
	{
		public static T GetComplexData<T>(this ISession session, string key)
		{
			var data = session.GetString(key);
			if (data == null)
			{
				return default;
			}
			return JsonConvert.DeserializeObject<T>(data);
		}

		public static void SetComplexData(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
	}
}