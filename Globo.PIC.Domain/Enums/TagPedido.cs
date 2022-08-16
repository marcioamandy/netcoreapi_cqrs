using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum TagPedido
	{
		[Description("-")]
		INDEFINIDA = 0,

		[Description("Estruturada")]
		ESTRUTURADA = 1,

		[Description("Externa")]
		EXTERNA = 2,

		[Description("Estruturada/ Externa")]
		MISTA = 3,

		[Description("Veículos de Cena")]
		VEICULOCENA = 4
	}
} 