using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globo.PIC.API.Middlewares;

namespace Globo.PIC.API.Extensions
{

	/// <summary>
	/// 
	/// </summary>
	public static class GlobalExceptionHandlerMiddlewareExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
		{
			return services.AddTransient<GlobalExceptionHandlerMiddleware>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="app"></param>
		public static void UseGlobalExceptionHandlerMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		}
	}
}
