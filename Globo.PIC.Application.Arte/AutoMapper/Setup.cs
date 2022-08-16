using System;
using Microsoft.Extensions.DependencyInjection;
using Globo.PIC.Application.Arte.Profiles;

namespace Globo.PIC.Application.Arte.AutoMapper
{
	/// <summary>
	/// 
	/// </summary>
	public static class Setup
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public static void AddAutoMapperApplicationArte(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(
				typeof(ModelToViewModel),
				typeof(EntityToViewModel),
				typeof(ViewModelToEntity));
		}
	}
}