using System;
using Microsoft.Extensions.DependencyInjection;
using Globo.PIC.Application.Veiculo.Profiles;

namespace Globo.PIC.Application.Veiculo.AutoMapper
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
		public static void AddAutoMapperApplicationVeiculo(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			services.AddAutoMapper(
				typeof(ModelToViewModel),
				typeof(EntityToViewModel),
				typeof(ViewModelToEntity));
		}
	}
}