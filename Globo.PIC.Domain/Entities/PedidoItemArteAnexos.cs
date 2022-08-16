using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class PedidoItemArteAnexos
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
        [Description("Id do Pedido Item Arte")]
        public long IdPedidoItemArte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Arquivo")]
        public string NomeArquivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Description("Nome Original")]
        public string NomeOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Caminho do Arquivo")]
        public string CaminhoArquivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo do Arquivo")]
        public string Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data do Arquivo")]
        public DateTime? DataPedidoItemArteAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PedidoItemArteAnexos()
        {
        }

        #region Relationship one to many properties

        
        #endregion

        #region Relationship many to one properties


        public virtual PedidoItemArte PedidoItemArte { get; set; }

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
