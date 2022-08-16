using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class PedidoItemVeiculoViewModel : PedidoItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Personagem Utilização")]
        [MaxLength(200)]
        public string PersonagemUtilizacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Modelo")]
        [MaxLength(200)]
        public string Modelo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Tipo de Veículos")]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da SubCategoria de Vewículos")]
        public long IdSubCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Número de Opções")]
        [Range(0, long.MaxValue)]
        public long NroOpcoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ano")]
        [Range(0, long.MaxValue)]
        public long? Ano { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cor")]
        [MaxLength(30)]
        public string Cor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Chegada")]
        public DateTime? DataChegadaVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolucao")]
        public DateTime? DataDevolucaoVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Continuidade")]
        public bool Continuidade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cena de Ação")]
        public bool CenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre a cena de ação")]
        [MaxLength(500)]
        public string SobreCenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Status")]
        public long IdStatus { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Observação do Pedido Veiculos Item")]
        [MaxLength(500)]
        public string ObservacaoVeiculo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de gravação do veículo")]
        [MaxLength(200)]
        public string LocalGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nro de passageiros")]
        [Range(0, long.MaxValue)]
        public long Passageiros { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Horas de voo")]
        [MaxLength(50)]
        public string HorasVoo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Horas com o veículo parado")]
        [MaxLength(50)]
        public string HorasParado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tag (Continuidade / Facilidade)")]
        public long Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Existe opcoes")]
        public bool? existeOpcoes
        {
            get
            {
                try
                {
                    if (ItensVeiculo == null || ItensVeiculo.Count <= 0)
                        return false;
                    else
                        return true;
                }
                catch
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("Existe acionamento")]
        public bool? existeAcionamento
        {
            get
            {
                try
                {
                    if (AcionamentoItens == null || AcionamentoItens.Count <= 0)
                        return false;
                    else
                        return true;
                }
                catch
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo (1 = Veículo de Pesquisa /2 = Veículo de Catálogo /3 = Veículo de Empréstimo)")]
        public long IdOrigem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Necessidades")]
        [MaxLength(500)]
        public string Necessidades { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Description("Valor Maximo")]
        public decimal ValorMaximo { get; set; }

        /// <summary>
		/// 
		/// </summary>
		[Description("Local de Faturamento")]
        [MaxLength(100)]
        public string LocalFaturamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data Devolver")]
        public DateTime? DataDevolver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Devolver")]
        [MaxLength(500)]
        public string JustificativaDevolver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Status Pedido Item Veículo")]
        public StatusViewModel Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo Item Veículo")]
        public CategoriaViewModel Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Subcategoria Item Veículo")]
        public CategoriaViewModel SubCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista ItemVeiculo")]
        public List<ItemVeiculoViewModel> ItensVeiculo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoItemVeiculoTracking")]
        public List<TrackingVeiculoViewModel> Tracking { get; set; }

    }
}
