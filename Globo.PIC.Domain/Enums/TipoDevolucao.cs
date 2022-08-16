using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
	public enum TipoDevolucao
	{
		[Description("DEMANDANTE")]
		ESTRUTURADA = 1,

		[Description("BASE")]
		EXTERNA = 2
	}
}
