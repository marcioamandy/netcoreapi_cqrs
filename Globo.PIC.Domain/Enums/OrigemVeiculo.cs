using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum OrigemVeiculo
	{

		[Description("Pesquisa")]
		VEICULOPESQUISA= 1,

		[Description("Catálogo")]
		VEICULOCATALOGO = 2,

		[Description("Empréstimo")]
		VEICULOEMPRESTIMO = 3
	}
}
