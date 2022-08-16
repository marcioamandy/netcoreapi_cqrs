using Globo.PIC.Domain.Interfaces;
using Globo.PIC.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Globo.PIC.API.Middlewares
{
	public class DefaultMiddleware
	{

		private readonly RequestDelegate _next;

		public DefaultMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			await _next(httpContext);
		}
	}
}