
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class ItemCatalogo
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(1, long.MaxValue)]
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
        [Range(1, long.MaxValue)]
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
        [MaxLength(500)]
        public string JustificativaBloqueio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Ativo no catálogo até")]
        public DateTime? AtivoAte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data inicial do item no catálogo")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        [Description("Data final do item no catálogo")]
        public DateTime? DataFim { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Ativo")]
        public bool Ativo { get; set; }

        #region Relationship one to many properties

        #endregion

        #region Relationship many to one properties

        public virtual Item Item { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>  
        public ItemCatalogo()
        {
        }
    }
}
