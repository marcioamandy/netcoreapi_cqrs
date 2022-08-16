using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemArteViewModel : PedidoItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens pendentes de compra")]
        public long QuantidadePendenteCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens pendentes de entrega")]
        public long QuantidadePendenteEntrega { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens entregues")]
        public long QuantidadeEntregue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens comprada")]
        public long QuantidadeComprada { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens devolvida")]
        public long QuantidadeDevolvida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Marcação de cena")]
        //[Range(0, long.MaxValue)]
        public bool MarcacaoCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Texto de referências do Item")]
        public string Referencias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sugestão de um fornecedor para a compra do Item")]
        public string SugestaoFornecedor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Solicitação dirigida do Item")]
        public bool SolicitacaoDirigida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Comprador")]
        public string CompradoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de vinculo do comprador")]
        public DateTime? DataVinculoComprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de visualização do comprador")]
        public DateTime? DataVisualizacaoComprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo do Pedido Item de Compra")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Reenvio")]
        public DateTime? DataReenvio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Edição Reenvio")]
        public DateTime? DataEdicaoReenvio { get; set; }

        ///novos campos 21/07/2020  
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Range(0, long.MaxValue)]
        [Description("Quantidade de itens compra aprovada")]
        public long QuantidadeAprovacaoCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data entrega prevista para")]
        public DateTime? DataEntregaPrevista { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação de aprovação da compra")]
        public string ObservacaoAprovacaoCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [Description("Flag Devolução de item para a base")]
        public bool FlagDevolvidoBase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        [Description("Flag Devolução de item para o comprador")]
        public bool FlagDevolvidoComprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Flag item não encontrado no catálogo")]
        public bool FlagItemNaoEncontrado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status Pedido Item")]
        public StatusViewModel Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UserViewModel")]
        public UsuarioViewModel Comprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido Item Arte Compras")]
        public List<PedidoItemArteCompraViewModel> Compras { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemArteDevolucao")]
        public List<PedidoItemArteDevolucaoViewModel> Devolucoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemArteEntrega")]
        public List<PedidoItemArteEntregaViewModel> Entregas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemArteTracking")]
        public List<TrackingArteViewModel> TrackingArte { get; set; }

    }
}
