using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
    public enum TipoPagamento
    {
        [Description("Diária")]
        DIARIA = 1,

        [Description("Pacote")]
        PACOTE = 2
    }
}
