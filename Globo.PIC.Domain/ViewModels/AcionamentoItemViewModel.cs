using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class AcionamentoItemViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id Acionamento ")]
        public long IdAcionamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Item")]
        public long IdPedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cena de Ação")]
        public bool CenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre a cena de ação")]
        public string SobreCenaAcao { get; set; }

        // <summary>
        /// 
        /// </summary>
        [Description("Local de Gravação")]
        [MaxLength(100)]
        public string LocalGravacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Passageiros")]
        public int Passageiros { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Horas de Voo")]
        public string HorasVoo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Horas Parado")]
        public string HorasParado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Taxar ?")]
        public bool Taxar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Local de Embarque")]
        [MaxLength(100)]
        public string LocalEmbarque { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Periodo de Utilizacao de Cena")]
        [MaxLength(100)]
        public string PeriodoUtilizacaoCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmação Cena de Ação")]
        public DateTime? DataConfirmacaoCenaAcao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Insulfilm")]
        public bool Insulfilm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre Insulfilm")]
        public string SobreInsulfilm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmação Insulfilm")]
        public DateTime? DataConfirmacaoInsulfilm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Adesivagem")]
        public bool Adesivagem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre Adesivagem")]
        public string SobreAdesivagem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmacao Adesivagem")]
        public DateTime? DataConfirmacaoAdesivagem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Mecânica")]
        public bool Mecanica { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre Mecânica")]
        public string SobreMecanica { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmacao Mecanica")]
        public DateTime? DataConfirmacaoMecanica { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Motorista de Cena")]
        public bool MotoristaCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre o Motorista de Cena")]
        public string SobreMotoristaCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmacao Motorista Cena")]
        public DateTime? DataConfirmacaoMotoristaCena { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Reboque")]
        public bool Reboque { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Sobre Reboque ")]
        public string SobreReboque { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Confirmacao Reboque")]
        public DateTime? DataConfirmacaoReboque { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Aprovacao")]
        public DateTime? DataAprovacao { get; set; }

        // <summary>
        /// 
        /// </summary>
        [Description("Login Aprovacao")]
        public string LoginAprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data Reprovacao")]
        public DateTime? DataReprovacao { get; set; }

        // <summary>
        /// 
        /// </summary>
        [Description("LoginReprovacao")]
        public string LoginReprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa Reprovacao ")]
        public string JustificativaReprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Cena de Ação")]
        public bool Cancelado { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa do Cancelamento")]
        [MaxLength(200)]
        public string JustificativaCancelamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Outra Justificativa ")]
        [MaxLength(500)]
        public string OutraJustificativa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista AcionamentoItem")]
        public List<ArquivoViewModel> Arquivos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("PedidoItem")]
        public PedidoItemViewModel PedidoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Acionamento")]
        public AcionamentoViewModel Acionamento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuário de Aprovação")]
        public UsuarioViewModel UsuarioAprovacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Usuário de Reprovação")]
        public UsuarioViewModel UsuarioReprovacao { get; set; }
    }
}
