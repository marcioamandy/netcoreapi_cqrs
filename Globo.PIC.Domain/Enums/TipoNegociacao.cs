using System.ComponentModel;

namespace Globo.PIC.Domain.Enums
{
    public enum TipoNegociacao
    {

        [Description("Acordo com item")]
        ACORDO_COM_ITEM = 1,

        [Description("Acordo sem item")]
        ACORDO_SEM_ITEM = 2,

        [Description("OC Mãe")]
        OC_MAE = 3,

        [Description("Ordem de Compra Padrão")]
        ORDER_COMPRA_PADRAO = 4
    }
}
