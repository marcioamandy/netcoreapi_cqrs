using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class ItemVeiculo
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item Veiculos")]
        [Range(1, long.MaxValue)]
        public long IdPedidoItemVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id Item")]
        [Range(1, long.MaxValue)]
        public long IdItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Placa")]
        [MaxLength(20)]
        public string Placa { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Cidade")]
        [MaxLength(100)]
        public string Cidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Chegada")]
        public DateTime? DataChegada { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolucao")]
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação Comprador")]
        [MaxLength(500)]
        public string ObservacaoComprador { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observação EP")]
        [MaxLength(500)]
        public string ObservacaoEP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Tipo de Pagamento (Diário ou Pacote)")]
        [Range(1, long.MaxValue)]
        public long IdTipoPagamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Modelo")]
        [MaxLength(200)]
        public string Modelo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ano")]
        public long Ano { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cor")]
        [MaxLength(30)]
        public string Cor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Período se o tipo de pagamento for selecionado o Pacote")]
        [MaxLength(200)]
        public string Periodo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data limite em que o item ficará ativo no catálogo")]
        public DateTime? DataAtivoAte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da aprovação do item")]
        public DateTime? DataAprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data da reprovação do item")]
        public DateTime? DataReprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa para a reprovação do veículo item")]
        [MaxLength(500)]
        public string JustificativaReprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Hora Fechamento")]
        [MaxLength(20)]
        public string HoraFechamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Confirmação Envio Aprovação")]
        public DateTime? DataConfirmacaoEnvioAprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Bloqueio de empréstimos")]
        public bool BloqueioEmprestimos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa para o bloqueio")]
        [MaxLength(500)]
        public string JustificativaBloqueio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ItemVeiculo()
        {
        }

        #region Relationship one to many properties

        #endregion

        #region Relationship one to one properties

        public virtual Item Item { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual PedidoItemVeiculo PedidoItemVeiculo { get; set; }

        

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
