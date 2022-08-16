using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class ItemCatalogoViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Veiculos Item")]
        public long IdItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Conteudo")]
        public long IdConteudo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Bloqueado para outros conteudos")]
        public bool BloqueadoOutrosConteudos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Justificativa para o bloqueio")]
        public string JustificativaBloqueio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ativo no catálogo até")]
        public DateTime? AtivoAte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data inicial do item no catálogo")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data final do item no catálogo")]
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ativo")]
        public bool Ativo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Item")]
        public ItemViewModel Item { get; set; }
    }
}
