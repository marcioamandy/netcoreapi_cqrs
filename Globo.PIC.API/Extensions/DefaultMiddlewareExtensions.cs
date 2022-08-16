using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Globo.PIC.API.Middlewares;

namespace Globo.PIC.API.Extensions
{

	/// <summary>
	/// 
	/// </summary>
	public static class DefaultMiddlewareExtensions
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddDefaultMiddleware(this IServiceCollection services)
		{
			return services.AddTransient<DefaultMiddleware>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		public static void UseDefaultMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<DefaultMiddleware>();
		}
	}
}
