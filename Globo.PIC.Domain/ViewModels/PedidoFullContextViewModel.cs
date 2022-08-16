using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoFullContextViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Pedido")]
        public DateTime? DataPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Description("Login do Solicitante")]
        public string LoginSolicitante { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login Autorização")]
        public string LoginAutorizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Autorizacao")]
        public DateTime? DataAutorizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        [Description("Número de itens do pedido")]
        public long NroItens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor total dos itens")]
        public long ValorItens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Necessário")]
        public DateTime? DataNecessario { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Titulo")]
        public string Titulo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do conteúdo")]
        public long? IdConteudo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Entrega")]
        public string LocalEntrega { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Descrição da Cena")]
        public string DescricaoCena { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação do Pedido")]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo do Pedido")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Comprador")]
        public string LoginComprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login Cancelamento")]
        public string LoginCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de cancelamento do pedido")]
        public DateTime? DataCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de confirmação da compra do pedido")]
        public DateTime? DataConfirmacaoCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nro da RC do pedido")]
        public string RcPedido { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Cancelamento do Pedido")]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login devolução")]
        public string LoginDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de devolução do pedido")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa devolução do pedido")]
        public string JustificativaDevolucao { get; set; }

        [Description("Lista de itens")]
        public List<PedidoItem> PedidoItens { get; set; }
        public PedidoFullContextViewModel()
        {

        }
    }
}
