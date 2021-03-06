﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce
{
	public static class Program
	{
		//public static void Main(string[] args)
		//{
		//	CreateWebHostBuilder(args).Build().Run();
		//}

		//public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
		//	WebHost.CreateDefaultBuilder(args)
		//		.UseStartup<Startup>();

		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
			//.ConfigureLogging(logging =>
			//{
			//	logging.ClearProviders();
			//	logging.AddConsole();
			//})
			.UseStartup<Startup>();
	}
}