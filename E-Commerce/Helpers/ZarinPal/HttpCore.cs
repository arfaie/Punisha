using System;
using System.IO;
using System.Net;

namespace ECommerce.Helpers.ZarinPal
{
	public class HttpCore
	{
		public HttpCore(string url, string method, string data)
		{
			Url = url;
			Method = method;
			Data = data;
		}

		public string Url { get; set; }
		public string Method { get; set; }
		public string Data { get; set; }

		public string GetResponse()
		{
			var webRequest = WebRequest.CreateHttp(Url);
			webRequest.Method = Method;
			if (Method.Equals("POST", StringComparison.CurrentCultureIgnoreCase))
			{
				webRequest.ContentType = "application/json";
				using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
				{
					streamWriter.Write(Data);
					streamWriter.Flush();
					streamWriter.Close();
				}
			}

			Stream responseStream = null;
			try
			{
				var webResponse = webRequest.GetResponse();
				responseStream = webResponse.GetResponseStream();
			}
			catch (WebException e)
			{
				if (e.Response != null)
					responseStream = e.Response.GetResponseStream();
			}
			if (responseStream == null) return null;

			string result;
			using (StreamReader streamReader = new StreamReader(responseStream))
			{
				result = streamReader.ReadToEnd();
				streamReader.Close();
			}
			return result;
		}
	}
}