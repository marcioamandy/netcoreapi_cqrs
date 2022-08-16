using System;
using Microsoft.Extensions.DependencyInjection;
using Globo.PIC.Application.Profiles;

namespace Globo.PIC.Application.AutoMapper
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
		public static void AddAutoMapperApplication(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(
				typeof(ModelToViewModel),
				typeof(EntityToViewModel),
				typeof(ViewModelToEntity));
		}
	}
}