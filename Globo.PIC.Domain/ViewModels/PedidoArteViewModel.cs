using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoArteViewModel : PedidoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Necessidade")]
        public DateTime? DataNecessidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Utilização")]
        public string LocalUtilizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descrição da Cena")]
        public string DescricaoCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de Solicitação do Cancelamento do pedido")]
        public DateTime? DataSolicitacaoCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de confirmação da compra do pedido")]
        public DateTime? DataConfirmacaoCompra { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login Base vinculado ao Pedido")]
        public string BaseLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data de vinculo da base ao pedido")]
        public DateTime? DataVinculoBase { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        [Description("Pedido de Alimentos")]
        public bool FlagPedidoAlimentos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("FastPass")]
        public bool FlagFastPass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Login do Comprador")]
        public string CompradoPorLogin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista Status do PedidoArte")]
        public StatusViewModel Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemArteViewModel")]
        public List<PedidoItemArteViewModel> Itens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuario da Base")]
        public UsuarioViewModel Base { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Description("Lista UsuarioViewModel")]
        public List<UsuarioViewModel> Compradores{ 
            get
            {
                if (this.Itens != null)
                {
                    return this.Itens.Where(a => a.Comprador != null)
                      .Select(a => a.Comprador).Distinct().ToList();
                }
                else return null;
            }
        }
    }
}
